using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class EntryPoint : MonoBehaviour {
    [SerializeField] private MainPageView _mainPageView;
    [SerializeField] private StorePageView _storePageView;
    [SerializeField] private GameplayView _gameplayView;
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private PigeonSpawner _pigeonSpawner;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private ScoresControll _scoresControll;
    [SerializeField] private AdsManager _adsManager;

    private GameStateMachine _gameStateMachine;

    public MainPageView MainPageView => _mainPageView;
    public StorePageView StorePageView => _storePageView;
    public GameplayView GameplayView => _gameplayView;
    public LevelGenerator LevelGenerator => _levelGenerator;
    public PigeonSpawner PigeonSpawner => _pigeonSpawner;
    public AudioMixer AudioMixer => _audioMixer;
    public PlayerData PlayerData => _playerData;
    public ScoresControll ScoresControll => _scoresControll;
    public AdsManager AdsManager => _adsManager;

    private void Awake() {
        //для дебага можно обнулить пользовательские данные, загрузка происходит в MainMenuState
        //_playerData.ZeroData();

        _mainPageView.Initialize();
        _storePageView.Initialize();
        _gameplayView.Initialize();

        //Инициализировать все игровые пулы заранее для быстрого старта геймплея
        _levelGenerator.Initialize(_scoresControll);

        //Запустить машину состояния игры - войти в главное меню
        _gameStateMachine = new GameStateMachine(this);
    }

    private void FixedUpdate() {
        _gameStateMachine.Update();
    }
}
