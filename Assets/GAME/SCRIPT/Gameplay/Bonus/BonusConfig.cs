using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusConfig", menuName = "LevelConfigs/BonusConfig")]
public class BonusConfig : ScriptableObject {
    [SerializeField] private List<Bonus> _bonusesPrefabs;

    public List<Bonus> BonusesPrefabs => _bonusesPrefabs;
}