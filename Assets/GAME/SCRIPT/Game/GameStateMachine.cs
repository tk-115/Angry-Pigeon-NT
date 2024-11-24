using System.Collections.Generic;
using System.Linq;

public class GameStateMachine : IStateSwicher {
    private List<IState> _allStates;
    private IState _currentState;

    public GameStateMachine(EntryPoint entryPoint) {
        _allStates = new List<IState>() {
            new MainMenuState(this, entryPoint.MainPageView, entryPoint.PlayerData, entryPoint.AudioMixer),
            new GameplayState(this, entryPoint.LevelGenerator, entryPoint.PigeonSpawner, entryPoint.PlayerData, entryPoint.GameplayView, entryPoint.ScoresControll, entryPoint.AdsManager),
            new StoreState(this, entryPoint.PlayerData, entryPoint.StorePageView, entryPoint.AdsManager)
        };

        _currentState = _allStates[0];
        _currentState.Enter();
    }

    public void SwitchState<T>() where T : IState {
        IState state = _allStates.FirstOrDefault(state => state is T);

        if (state == null) throw new System.ArgumentOutOfRangeException("Required state not finded");

        _currentState.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    public void Update() => _currentState.Update();
}
