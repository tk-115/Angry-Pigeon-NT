using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
public class BonusSpawner : MonoBehaviour {

    private Animator _animator; 
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private List<Transform> _coinsSpawnPoints;

    [Inject] private BonusFactory _bonusFactory;

    private Bonus _spawnedBonus;

    private List<BonusCoin> _spawnedCoins;

    private void Awake() { _animator = GetComponent<Animator>(); }

    public void SpawnBonus() {

        //Спавним раномный бонус
        int bonusId = Random.Range(0, 120);
        if (bonusId < 30) _spawnedBonus = _bonusFactory.Get<BonusFullHP>();
        if (bonusId >= 30 && bonusId < 50) _spawnedBonus = _bonusFactory.Get<BonusProtection>();
        if (bonusId >= 50 && bonusId < 70) _spawnedBonus = _bonusFactory.Get<BonusMultiplier>();
        if (bonusId >= 70 && bonusId < 95) _spawnedBonus = _bonusFactory.Get<BonusSlowdown>();

        if (_spawnedBonus != null) {
            _animator.Play("BonusFly");
            _spawnedBonus.transform.parent = _spawnPoint;
            _spawnedBonus.transform.SetPositionAndRotation(_spawnPoint.position, Quaternion.identity);
        }

        //Спавним монеты - рандомное количество в рандомно выбранных spawnPointах
        _spawnedCoins = new List<BonusCoin>();

        //Вероятность выпадения монетки - рандом
        for (int i = 0; i < _coinsSpawnPoints.Count; i++) {
            int probabilityOfCoinSpawn = UnityEngine.Random.Range(0, 100);
            int spawnOrnot = UnityEngine.Random.Range(0, 100);
            if (spawnOrnot < probabilityOfCoinSpawn) {
                BonusCoin newCoin = _bonusFactory.Get<BonusCoin>();
                if (newCoin != null) {
                    newCoin.transform.parent = _coinsSpawnPoints[i];
                    newCoin.transform.SetPositionAndRotation(_coinsSpawnPoints[i].position, Quaternion.identity);
                    newCoin.Initialize();
                    _spawnedCoins.Add(newCoin);
                }
            }
        }
    }

    public void Shutdown() {
        //Очистка бонуса
        if (_spawnedBonus != null && _spawnedBonus.IsPickedUp == false) {
            _animator.Play("IDLE");
            Destroy(_spawnedBonus.gameObject);
        }
        //Очистка монет
        if (_spawnedCoins.Count > 0) {
            for (int i = 0; i < _spawnedCoins.Count; i++) {
                if (_spawnedCoins[i].IsPickedUp == false) Destroy(_spawnedCoins[i].gameObject);
            }
        }
    }
}