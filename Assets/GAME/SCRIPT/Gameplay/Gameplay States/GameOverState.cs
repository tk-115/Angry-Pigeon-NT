using UnityEngine;
using Zenject;

public class GameOverState : IState {
    private IStateSwicher _stateSwicher;
    private GameplayView _gameplayView;
    private LevelGenerator _levelGenerator;
    private ScoresControll _scoresControll;
    private IStateSwicher _gameStateSwitcher;
    private PigeonMain _pigeonMain;
    private AdsManager _adsManager;  

    private bool _resurrectionUsed = false;

    public GameOverState(IStateSwicher stateSwicher, GameplayView gameplayView, 
        LevelGenerator levelGenerator, ScoresControll scoresControll, IStateSwicher gameStateSwitcher, PigeonMain pigeonMain, AdsManager adsManager) {
        _stateSwicher = stateSwicher;
        _gameplayView = gameplayView;
        _levelGenerator = levelGenerator;
        _scoresControll = scoresControll;
        _gameStateSwitcher = gameStateSwitcher;
        _pigeonMain = pigeonMain;
        _adsManager = adsManager;

        _resurrectionUsed = false;
    }

    public void Enter() {
        if (_resurrectionUsed == true)
        _gameplayView.SetRessurectionButtonText(_gameplayView.LanguageControll.GetLocaledTextByKey("go_resurrection_used"));
        else _gameplayView.SetRessurectionButtonText(_gameplayView.LanguageControll.GetLocaledTextByKey("go_resurrection"));

        //EVENTЫ кнопок у меню паузы
        _gameplayView.OnButtonRestartClickEvent += OnButtonRestartClicked;
        _gameplayView.OnButtonInMainMenuClickEvent += OnButtonToMainMenuClicked;
        _gameplayView.OnButtonResurrectionClickEvent += OnButtonResurrectionClicked;
        _levelGenerator.GameOverLogic();
        _gameplayView.ShowGameOverPage();

        _gameplayView.SetTotalScores(_scoresControll.GetScore());
        _gameplayView.SetTotalMeters(_scoresControll.GetMeter());
        _gameplayView.SetTotalCoins(_scoresControll.GetCoins());

        //результат сохранения текущего рекорда показывает - есть ли в каком-либо рекорде рекорд
        _gameplayView.SetRecordOrNot(_scoresControll.SaveRecords());
    }

    private void OnButtonResurrectionClicked() {
        if (_resurrectionUsed == false) {
            //отключаем кнопки что бы юзер не тыкал
            _gameplayView.OnButtonRestartClickEvent -= OnButtonRestartClicked;
            _gameplayView.OnButtonInMainMenuClickEvent -= OnButtonToMainMenuClicked;
            _gameplayView.OnButtonResurrectionClickEvent -= OnButtonResurrectionClicked;
            //сообщить юзеру что реклама грузится
            _gameplayView.SetRessurectionButtonText(_gameplayView.LanguageControll.GetLocaledTextByKey("go_resurrection_loading"));
            //ads events
            _adsManager.OnRewarderAdComplete += OnRewardedAdOk;
            _adsManager.OnRewarderAdFailed += OnRewardedAdFailed;
            //launch ad
            _adsManager.LoadRewardedAd();
        }
    }

    private void OnRewardedAdOk() {
        _resurrectionUsed = true;
        _levelGenerator.ResurrectionLogic();
        _pigeonMain.Respawn();
        _stateSwicher.SwitchState<GameplayMainState>();
    }

    private void OnRewardedAdFailed() {
        //что-то пошло не так, сообщить об этом юзеру
        _gameplayView.SetRessurectionButtonText(_gameplayView.LanguageControll.GetLocaledTextByKey("go_resurrection_failed"));
        //кнопки снова доступны
        _gameplayView.OnButtonRestartClickEvent += OnButtonRestartClicked;
        _gameplayView.OnButtonInMainMenuClickEvent += OnButtonToMainMenuClicked;
        _gameplayView.OnButtonResurrectionClickEvent += OnButtonResurrectionClicked;
    }

    private void OnButtonToMainMenuClicked() {
        _gameplayView.OnButtonRestartClickEvent -= OnButtonRestartClicked;
        _gameplayView.OnButtonInMainMenuClickEvent -= OnButtonToMainMenuClicked;
        _gameplayView.OnButtonResurrectionClickEvent -= OnButtonResurrectionClicked;

        //save records

        _gameStateSwitcher.SwitchState<MainMenuState>();
    }

    private void OnButtonRestartClicked() {
        _scoresControll.SaveRecords();
        _levelGenerator.RebuildLevel();

        _gameplayView.OnButtonRestartClickEvent -= OnButtonRestartClicked;
        _gameplayView.OnButtonInMainMenuClickEvent -= OnButtonToMainMenuClicked;
        _gameplayView.OnButtonResurrectionClickEvent -= OnButtonResurrectionClicked;

        _pigeonMain.Respawn();

        _resurrectionUsed = false;

        _stateSwicher.SwitchState<GameplayMainState>();
    }

    public void Exit() {
        _gameplayView.OnButtonRestartClickEvent -= OnButtonRestartClicked;
        _gameplayView.OnButtonInMainMenuClickEvent -= OnButtonToMainMenuClicked;
        _gameplayView.OnButtonResurrectionClickEvent -= OnButtonResurrectionClicked;
        _gameplayView.HideGameOverPage();
    }

    public void Update() { }
}
