using UnityEngine;
using Zenject;

public class PigeonFactory {
    private const string PIGEONS_CONFIG = "AllPigeonsCfg";

    private PigeonConfig _pigeonConfig;
    private IInstantiator _container;

    public PigeonFactory(IInstantiator container) {
        _container = container;
        _pigeonConfig = Resources.Load<PigeonConfig>(PIGEONS_CONFIG);
    }

    public PigeonMain GetPigeon(int pigeonID, Vector3 spawnPoint) {
        PigeonMain pigeon = _container.InstantiatePrefabForComponent<PigeonMain>(_pigeonConfig.PigeonPrefabs[pigeonID].PigeonMain, spawnPoint, Quaternion.identity, null);

        //= GameObject.Instantiate(_pigeonConfig.PigeonPrefabs[pigeonID].PigeonMain, spawnPoint, Quaternion.identity);
        pigeon.Bomber.Initialize(_pigeonConfig.PigeonPrefabs[pigeonID].AvailableBomb);
        return pigeon;
    }
}