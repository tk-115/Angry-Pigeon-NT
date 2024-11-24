using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockConfig", menuName = "LevelConfigs/BlockConfig")]
public class BlockConfig : ScriptableObject {
    [SerializeField] private List<Block> _blocksPrefabs;
    [SerializeField] private List<Pedestrian> _pedestriansPrefabls;
    [SerializeField] private Rogatk _rogatkPrefab;
    [SerializeField] private PVO _pvoPrefab;

    public List<Block> BlocksPrefabs => _blocksPrefabs;
    public List<Pedestrian> PedestriansPrefabs => _pedestriansPrefabls;
    public Rogatk RogatkPrefab => _rogatkPrefab;
    public PVO PVOPrefab => _pvoPrefab; 
}