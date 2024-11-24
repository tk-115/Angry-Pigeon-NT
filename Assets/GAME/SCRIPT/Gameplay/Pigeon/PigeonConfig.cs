using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PigeonUnit {
    public PigeonMain PigeonMain;
    public Bomb AvailableBomb;
}

[CreateAssetMenu(fileName = "PigeonConfig", menuName = "LevelConfigs/PigeonConfig")]
public class PigeonConfig : ScriptableObject {
    [SerializeField] private List<PigeonUnit> _pigeonPrefabs;

    public List<PigeonUnit> PigeonPrefabs => _pigeonPrefabs;
}
