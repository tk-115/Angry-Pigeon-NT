using UnityEngine;
using Zenject;

public class BonusFactory {
    private const string BONUS_CONFIG = "AllBonusesCfg";
    private BonusConfig _blockConfig;
    private IInstantiator _container;

    public BonusFactory(IInstantiator container) {
        _container = container;
        Load();
    }

    public T Get<T>() where T: Bonus {
        for (int i = 0; i < _blockConfig.BonusesPrefabs.Count; i++) {
            if (_blockConfig.BonusesPrefabs[i] is T) {
                Bonus newBonus = _container.InstantiatePrefabForComponent<Bonus>(_blockConfig.BonusesPrefabs[i]);
                return newBonus as T;
            }
        }
        return null;
    }

    private void Load() => _blockConfig = Resources.Load<BonusConfig>(BONUS_CONFIG);
}