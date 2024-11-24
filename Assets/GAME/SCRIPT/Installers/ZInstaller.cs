using UnityEngine;
using Zenject;

public class ZInstaller : MonoInstaller {
    [SerializeField] private GameplayView _gameplayView;
    [SerializeField] private ScoresControll _scoresControll;
    [SerializeField] private LevelGenerator _levelGenerator;

    public override void InstallBindings() {
        Container.Bind<LanguageControll>().AsSingle();
        Container.BindInstance<GameplayView>(_gameplayView).AsSingle();
        Container.BindInstance<ScoresControll>(_scoresControll).AsSingle();
        Container.BindInstance<LevelGenerator>(_levelGenerator).AsSingle();
        Container.Bind<PigeonFactory>().AsSingle();
        Container.Bind<BlockFactory>().AsSingle();
        Container.Bind<BonusFactory>().AsTransient();
    }
}