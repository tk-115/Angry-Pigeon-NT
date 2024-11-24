using System.Collections.Generic;
using UnityEngine;

public class BombPool {
    private const int INITIALIZE_POOL_AMOUNT = 5;

    private List<Bomb> _pooledBombs = new List<Bomb>();

    private Transform _spawnPoint;
    private Bomb _bombPrefab;

    public BombPool(Bomb bombPrefab, Transform spawnPoint) {
        _spawnPoint = spawnPoint;
        _bombPrefab = bombPrefab;
        for (int i = 0; i < INITIALIZE_POOL_AMOUNT; i++) {
            Bomb newBomb = GameObject.Instantiate(bombPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
            newBomb.InitializeParent(spawnPoint);
            newBomb.gameObject.SetActive(false);
            _pooledBombs.Add(newBomb);
        }
    }

    public Bomb GetBomb(bool instantiateIfNotAvailable) {
        for (int i = 0; i < _pooledBombs.Count; i++) {
            if (_pooledBombs[i].gameObject.activeInHierarchy == false) {
                _pooledBombs[i].gameObject.SetActive(true);
                return _pooledBombs[i];
            }
        }
        if (instantiateIfNotAvailable == true) {
            Bomb newBomb = GameObject.Instantiate(_bombPrefab, _spawnPoint.position, Quaternion.identity, _spawnPoint);
            _pooledBombs.Add(newBomb);
            return newBomb;
        }
        else {
            return null; 
        }
    }
}