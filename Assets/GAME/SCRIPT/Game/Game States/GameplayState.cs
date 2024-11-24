using UnityEngine;

public class GameplayState : IState {
    private IStateSwicher _switcher;
    private LevelGenerator _levelGenerator;
    private PigeonSpawner _pigeonSpawner;
    private PlayerData _playerData;
    private GameplayView _gameplayView; 
    private ScoresControll _scoresControll;
    private AdsManager _adsManager;

    private GameplayStateMachine _gameplayStateMachine;
    private PigeonMain _pigeonMain;

    public GameplayState(IStateSwicher switcher, LevelGenerator levelGenerator, 
        PigeonSpawner pigeonSpawner, PlayerData playerData,
        GameplayView gameplayView, ScoresControll scoresControll, AdsManager adsManager) {
        _switcher = switcher;
        _levelGenerator = levelGenerator;
        _pigeonSpawner = pigeonSpawner;
        _playerData = playerData;
        _gameplayView = gameplayView;
        _scoresControll = scoresControll;
        _adsManager = adsManager;
    }

    public void Enter() {
        //Инициализируем View Gameplay
        _gameplayView.UpdateLocalizationTable();

        //Запускаем генератор уровня 
        _levelGenerator.SetupLevel();

        //Создаем выбранного игроком в магазине голубя и иницииализиуем его в фабрике
        _pigeonMain = _pigeonSpawner.SpawnPigeonByID(_playerData.SelectedPigeonID);

        //Инициализируем голубя
        _pigeonMain.Initialize(_gameplayView, _pigeonSpawner.SpawnPoint);

        //Запустить свой GamePlayStateMachine
        _gameplayStateMachine = new GameplayStateMachine(_switcher, _pigeonMain, _gameplayView, _levelGenerator, _scoresControll, _adsManager);

        //УБрать фон главного меню
        _gameplayView.ShowGameplay();
    }

    public void Exit() {
        //Генератор уровня очистить
        _levelGenerator.ShutdownGameplay();

        //Если у голубя есть активный бонус, завершить его работу
        _pigeonMain.BonusControl.TerminateCurrentBonus();

        //Голубя уничтожить
        GameObject.Destroy(_pigeonMain.gameObject);
    }

    public void Update() { }
}
