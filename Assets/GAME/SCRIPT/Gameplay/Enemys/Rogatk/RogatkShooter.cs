using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rogatk))]
public class RogatkShooter : MonoBehaviour {
    private const int COUNT_OF_STONES = 4;
    private const int POWER_MIN = 10;
    private const int POWER_MAX = 13;
    private const int DEGREE_MIN = 120;
    private const int DEGREE_MAX = 135;
    private const float START_ATTACK_SCREEN_X_OFFSET = 3f;

    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _spawnPoint;

    private Rogatk _rogatk;
    private List<Projectile> _stones = new List<Projectile>();
    private int _stonesLeft;
    private float _starkAttackScreenPointX;

    public float StarkAttackScreenPointX => _starkAttackScreenPointX;

    public void Initialize() {
        _rogatk = GetComponent<Rogatk>();
        for (int i = 0; i < COUNT_OF_STONES; i++) {
            Projectile newProjectile = Instantiate(_projectilePrefab, _spawnPoint.position, Quaternion.identity, transform);
            newProjectile.Hide();
            _stones.Add(newProjectile);
        }
        _stonesLeft = _stones.Count;
        InitizlizeStartAttackScreenPointX();
    }

    private void InitizlizeStartAttackScreenPointX() { 
        Camera mainCamera = Camera.main;
        //Определяем точную координату по X крайней правой стороны экрана в соответствии с текущим разрешением экрана
        Vector3 worldViewport = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0));
        //Сохраняем координату X, при которой рогатчик начинает стрельбу
        _starkAttackScreenPointX = worldViewport.x + START_ATTACK_SCREEN_X_OFFSET;
    }

    public bool HasStones() { if (_stonesLeft > 0) return true; else return false; }

    public void Shoot() {
        if (_stonesLeft > 0) {
            _stonesLeft -= 1;
            _stones[_stonesLeft].Show();

            float _velx = Random.Range(POWER_MIN, POWER_MAX) * Mathf.Cos(Random.Range(DEGREE_MIN, DEGREE_MAX) * Mathf.Deg2Rad);
            float _vely = Random.Range(POWER_MIN, POWER_MAX) * Mathf.Sin(Random.Range(DEGREE_MIN, DEGREE_MAX) * Mathf.Deg2Rad);
            _stones[_stonesLeft].ApplyVelocity(new Vector2(_velx, _vely));

        }
        //Если это был последний, сообщить об этом в главком
        if (_stonesLeft == 0) _rogatk.SetIDLE();
    }

    public void Reload() {
        _stonesLeft = 0;
        for (int i = 0; i < _stones.Count; i++) {
            _stones[i].Reinstall();
            _stones[i].transform.SetPositionAndRotation(_spawnPoint.position, Quaternion.identity);
            _stonesLeft++;
        }
    }
}