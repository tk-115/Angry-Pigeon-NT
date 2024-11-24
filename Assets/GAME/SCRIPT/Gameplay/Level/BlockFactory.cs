using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BlockFactory {
    private const string BLOCKS_CONFIG = "AllBlocksCfg";

    private BlockConfig _blockConfig;
    private IInstantiator _container;
    private int _blockFromConfigCount;

    public BlockFactory(IInstantiator container) {
        _container = container;
        Load();
    }

    public int BlocksFromConfigCount => _blockFromConfigCount;

    public Block[] GetInitializedBlocks(int eachBlockAmount) {
        List<Block> initializedBlocks = new List<Block>();
        for (int i = 0; i < _blockConfig.BlocksPrefabs.Count; i++) {
            for (int j = 0; j < eachBlockAmount; j++) {
                Block newBlock = _container.InstantiatePrefabForComponent<Block>(_blockConfig.BlocksPrefabs[i]);
                
                //Pedestrians init in current block
                for (int q = 0; q < newBlock.PedestrianPaths.Count; q++) {
                    Pedestrian newPedestrian = _container.InstantiatePrefabForComponent<Pedestrian>(_blockConfig.PedestriansPrefabs[Random.Range(0, _blockConfig.PedestriansPrefabs.Count)], newBlock.transform);
                    newPedestrian.Initialize(newBlock.PedestrianPaths[q].navPoints);
                    newPedestrian.SetPosition(newBlock.PedestrianPaths[q].navPoints[0].position);
                    newBlock.AddInitializedPedestrian(newPedestrian);
                }

                //Rogatk init in current block
                Rogatk newRogatk = _container.InstantiatePrefabForComponent<Rogatk>(_blockConfig.RogatkPrefab, newBlock.transform);
                newRogatk.Initialize();
                newBlock.AddInitializedRogatk(newRogatk);

                //PVO init in current block if spawn point available
                //не все блоки могут размещать в себе ПВО
                if (newBlock.PVOSpawnPoint != null) {
                    PVO newPVO = _container.InstantiatePrefabForComponent<PVO>(_blockConfig.PVOPrefab, newBlock.transform);
                    newPVO.Initialize(newBlock.PVOSpawnPoint.position);
                    newBlock.AddInitializedPVO(newPVO);
                }
                
                //other enemys init in current block

                initializedBlocks.Add(newBlock);
            }
        }
        //save count of unique blocks from config
        _blockFromConfigCount = _blockConfig.BlocksPrefabs.Count;
        return initializedBlocks.ToArray();
    }

    private void Load() => _blockConfig = Resources.Load<BlockConfig>(BLOCKS_CONFIG);
}