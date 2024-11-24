using System.Collections.Generic;
using UnityEngine;

public class VoronaSpawner : MonoBehaviour {
    private const int START_SPAWN_METERS = 130;
    private const int SPAWN_ADD_PROBABILITY_COEFFICIENT = 5;
    private const float SPAWN_OFFSET = 3f;
    private const float ALERT_OFFSET = 1.3f;

    [SerializeField] private GameplayView _gameplayView;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private Vorona _voronaPrefab;

    private class VoronaSpawnPoint {
        public VoronaSpawnPoint(Vector3 voronaSpawnPoint, Vector3 alertUIposition) {
            VoronaSpawnPosition = voronaSpawnPoint;
            AlertUIPosition = alertUIposition;
        }

        public Vector3 VoronaSpawnPosition;
        public Vector3 AlertUIPosition;
    }

    private int _spawnProbability = 35;    
    private List<VoronaSpawnPoint> _startupPoints = new List<VoronaSpawnPoint>();
    private int _selectedSpawnPoint = 0;
    private bool _isSpawning = false;
    private Vorona _vorona;

    private void Awake() { 
        Camera mainCamera = Camera.main;
        //������������ ������� ������ ������ ��� �������� ���������� ������
        //� ��������� ������ ������� ������ - ������� ������ ��� UI

        //�� ���� �� _spawnPoints ������� ������ Y, � X� ����������� �������� ����������

        //������� ������ ����� �� ������ � ������� �����������
        Vector3 worldViewport = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0));
        
        for (int i = 0; i < _spawnPoints.Count; i++) {
            Vector3 spawnPosition = new Vector3(worldViewport.x + SPAWN_OFFSET, _spawnPoints[i].position.y, 0);

            Vector3 AlertPosition = mainCamera.WorldToScreenPoint(new Vector3(worldViewport.x - ALERT_OFFSET, _spawnPoints[i].position.y, 0));

            _startupPoints.Add(new VoronaSpawnPoint(spawnPosition, AlertPosition));
        }

        //���������������� ������ � �������� �
        _vorona = Instantiate(_voronaPrefab);
        _vorona.gameObject.SetActive(false);
    }

    public void SpawnVorona(int meters) {
        if (meters < START_SPAWN_METERS) return;

        //���� ������� ����, �������� ������ �� ����
        if (_isSpawning == true) return;

        //������� ������ �������
        _isSpawning = true;

        int spawnOrNot = Random.Range(0, 100);

        if (spawnOrNot < _spawnProbability) {
            //�������� ���������� ������� ������
            _selectedSpawnPoint = Random.Range(0, _startupPoints.Count);

            //������� ��������� ������ �� ��������� � ��������� �������
            _gameplayView.OnVoronaAlertComplete += OnAlertComplete;
            _gameplayView.ShowVoronaAlert(_startupPoints[_selectedSpawnPoint].AlertUIPosition);
        } else {
            //����������� �� ������, ������� �� ��������
            _isSpawning = false;
        }
        //��������� ����������� ������ ��� ��������� ��������
        if (_spawnProbability < 100) _spawnProbability += SPAWN_ADD_PROBABILITY_COEFFICIENT;
    }

    private void OnVoronaFlyComplete() {
        _gameplayView.OnVoronaAlertComplete -= OnAlertComplete;
        _vorona.OnVoronaFlyedEvent -= OnVoronaFlyComplete;
        _vorona.gameObject.SetActive(false);
        _isSpawning = false;
    }

    private void OnAlertComplete() {
        _vorona.gameObject.SetActive(true);
        _vorona.OnVoronaFlyedEvent += OnVoronaFlyComplete;
        _vorona.StartFly(_startupPoints[_selectedSpawnPoint].VoronaSpawnPosition);
    }
}
