using UnityEngine;
using Zenject;

public class PauseState : IState {
    private IStateSwicher _stateSwicher;
    private PigeonMain _pigeonMain;
    private GameplayView _gameplayView;
    private IStateSwicher _upperSwitcher;
    private LevelGenerator _levelGenerator;
    private ScoresControll _scoresControll;

    public PauseState(IStateSwicher stateSwicher, 
        IStateSwicher upperSwitcher, PigeonMain pigeonMain, GameplayView gameplayView, 
        LevelGenerator levelGenerator, ScoresControll scoresControll) {

        _stateSwicher = stateSwicher;
        _pigeonMain = pigeonMain;
        _gameplayView = gameplayView;
        _upperSwitcher = upperSwitcher;
        _levelGenerator = levelGenerator;
        _scoresControll = scoresControll;
    }

    public void Enter() {
        //смена статуса у голубя на IDLE
        _pigeonMain.StateMachine.SwitchState<PigeonIDLEState>();
        Time.timeScale = 0f;

        //EVENTЫ кнопок у меню паузы
        _gameplayView.OnButtonContinueClickEvent += OnButtonContinueClicked;
        _gameplayView.OnButtonRestartClickEvent += OnButtonRestartClicked;
        _gameplayView.OnButtonInMainMenuClickEvent += OnButtonToMainMenuClicked;
    }

    private void OnButtonContinueClicked() => _stateSwicher.SwitchState<GameplayMainState>();
    
    private void OnButtonRestartClicked() {
        Time.timeScale = 1f;
        _scoresControll.SaveRecords();
        _levelGenerator.RebuildLevel();
        _stateSwicher.SwitchState<GameplayMainState>();
    }

    private void OnButtonToMainMenuClicked() {
        _gameplayView.OnButtonContinueClickEvent -= OnButtonContinueClicked;
        _gameplayView.OnButtonRestartClickEvent -= OnButtonRestartClicked;
        _gameplayView.OnButtonInMainMenuClickEvent -= OnButtonToMainMenuClicked;
        
        _scoresControll.SaveRecords();
        _upperSwitcher.SwitchState<MainMenuState>();
    }

    public void Exit() {
        _gameplayView.OnButtonContinueClickEvent -= OnButtonContinueClicked;
        _gameplayView.OnButtonRestartClickEvent -= OnButtonRestartClicked;
        _gameplayView.OnButtonInMainMenuClickEvent -= OnButtonToMainMenuClicked;
        Time.timeScale = 1f;
        _gameplayView.HidePausePage();
    }

    void IState.Update() {
        //Обработка клавиш - отмена паузы, перемещение по пунктам меню
    }
}
