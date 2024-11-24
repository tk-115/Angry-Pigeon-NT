using System.Collections.Generic;
using System.Linq;

public class GameplayStateMachine : IStateSwicher {
    private List<IState> _allStates;
    private IState _currentState;

    public GameplayStateMachine(GameStateMachine gameStateSwicher, PigeonMain pigeonMain, 
        GameplayView gameplayView, LevelGenerator levelGenerator, ScoresControll scoresControll, AdsManager adsManager) {
        
        _allStates = new List<IState>() {
            new GameplayMainState(this, pigeonMain, gameplayView),
            new PauseState(this, gameStateSwicher, pigeonMain, gameplayView, levelGenerator, scoresControll),
            new GameOverState(this, gameplayView, levelGenerator, scoresControll, gameStateSwicher, pigeonMain, adsManager)
        };

        pigeonMain.DamageProcessor.Died += OnPigeonDied;

        _currentState = _allStates[0];
        _currentState.Enter();
    }

    private void OnPigeonDied() => SwitchState<GameOverState>();
  
    public void SwitchState<T>() where T : IState {
        IState state = _allStates.FirstOrDefault(state => state is T);

        if (state == null) throw new System.ArgumentOutOfRangeException("Required state not finded");

        _currentState.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    public void Update() => _currentState.Update();
}