using UnityEngine;
using Zenject;

public class ZInstaller : MonoInstaller
{
    //[SerializeField] private PigeonView _pigeonView;
    //[SerializeField] private PigeonDamageProcessor _damageProcessor;

    //[SerializeField] private PigeonMain _playerPrefab;
    //[SerializeField] private Transform _playerSpawnPoint;

    [SerializeField] private GameplayView _gameplayView;

    [SerializeField] private ScoresControll _scoresControll;

    [SerializeField] private LevelGenerator _levelGenerator;

    //public GameObject Player;

    public override void InstallBindings()
    {
        //PigeonMain player = Container.InstantiatePrefabForComponent<PigeonMain>(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity, null);
        //Container.BindInterfacesAndSelfTo<PigeonMain>().FromInstance(player).AsSingle();

        //Container.Bind<PigeonFactory>().AsSingle();

        Container.Bind<LanguageControll>().AsSingle();

        Container.BindInstance<GameplayView>(_gameplayView).AsSingle();

        Container.BindInstance<ScoresControll>(_scoresControll).AsSingle();

        Container.BindInstance<LevelGenerator>(_levelGenerator).AsSingle();

        Container.Bind<PigeonFactory>().AsSingle();

        Container.Bind<BlockFactory>().AsSingle();

        Container.Bind<BonusFactory>().AsTransient();

        //Container.BindInstance<GameObject>(Player).AsSingle();

        //Container.BindInstance<PigeonView>(_pigeonView).AsSingle().NonLazy();
        //  Bind<PigeonView>().AsSingle().NonLazy();
        //Container.Bind<PigeonDamageProcessor>().AsSingle().NonLazy();
        //Container.BindInstance<PigeonDamageProcessor>(_damageProcessor).AsSingle().NonLazy();
    }
}