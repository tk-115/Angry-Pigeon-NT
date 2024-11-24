using System.Collections.Generic;
using UnityEngine;

public class TripleBomb : Bomb {
    [SerializeField] private List<TripleBombUnit> _bombs;

    public override void InitializeParent(Transform parent) {
        for (int i = 0; i < _bombs.Count; i++) _bombs[i].SetDefaultParent(parent);
    }

    public override void Drop() {
        for (int i = 0; i < _bombs.Count; i++) _bombs[i].Drop();
    }
}

