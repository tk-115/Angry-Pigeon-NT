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
        //�������������� View Gameplay
        _gameplayView.UpdateLocalizationTable();

        //��������� ��������� ������ 
        _levelGenerator.SetupLevel();

        //������� ���������� ������� � �������� ������ � �������������� ��� � �������
        _pigeonMain = _pigeonSpawner.SpawnPigeonByID(_playerData.SelectedPigeonID);

        //�������������� ������
        _pigeonMain.Initialize(_gameplayView, _pigeonSpawner.SpawnPoint);

        //��������� ���� GamePlayStateMachine
        _gameplayStateMachine = new GameplayStateMachine(_switcher, _pigeonMain, _gameplayView, _levelGenerator, _scoresControll, _adsManager);

        //������ ��� �������� ����
        _gameplayView.ShowGameplay();
    }

    public void Exit() {
        //��������� ������ ��������
        _levelGenerator.ShutdownGameplay();

        //���� � ������ ���� �������� �����, ��������� ��� ������
        _pigeonMain.BonusControl.TerminateCurrentBonus();

        //������ ����������
        GameObject.Destroy(_pigeonMain.gameObject);
    }

    public void Update() { }
}
