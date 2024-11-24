using UnityEngine;

public class ScoresControll : MonoBehaviour {

    [SerializeField] private GameplayView _gameplayView;
    [SerializeField] private PlayerData _playerData;

    private int _score = 0;
    private int _coin = 0;
    private int _meters = 0;

    private int multiplier = 1;

    public void SetMultiplierActive() => multiplier = 2;

    public void SetMultiplierDefault() => multiplier = 1;

    public void AddScore(int value) {
        _score += value * multiplier;
        _gameplayView.SetScores(_score);
    }

    public int GetScore() { return _score; }

    public void ResetScores() {
        _score = 0;
        _gameplayView.SetScores(_score);
    }

    public void AddMeter() {
        _meters++;
        _gameplayView.SetMeters(_meters);
    }

    public int GetMeter() { return _meters; }

    public void ResetMeters() {
        _meters = 0;
        _gameplayView.SetMeters(_meters);
    }

    public void AddCoin() {
        _coin += multiplier;
        _gameplayView.SetCoins(_coin);
    }

    public int GetCoins() { return _coin; }

    public void ResetCoin() {
        _coin = 0;
        _gameplayView.SetCoins(_coin);
    }

    public bool SaveRecords() {
        bool newRecord = false;
        if (_playerData.Record_Scores < _score) {
            _playerData.Record_Scores = _score;
            newRecord = true;
        }
        if (_playerData.Record_Meters < _meters) {
            _playerData.Record_Meters = _meters;
            newRecord = true;
        }
        if (_playerData.Coins < _coin)  {
            _playerData.Coins = _coin;
            newRecord = true;
        }
        if (newRecord == true) _playerData.SaveData();
        return newRecord;
    }

}