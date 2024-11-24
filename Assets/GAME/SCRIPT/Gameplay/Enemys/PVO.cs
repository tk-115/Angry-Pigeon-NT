using UnityEngine;
using Zenject;

[RequireComponent(typeof(DamageProcessor))]
[RequireComponent (typeof(AudioSource))]    
public class PVO : MonoBehaviour {
    private const float START_ATTACK_SCREEN_X_OFFSET = 1;
    private const float ROCKET_POWER = 14;
    private const float ROCKET_ANGLE = 140;

    [Inject] private ScoresControll _scoresControll;

    [SerializeField] ScoresAddView _scoresAddView;
    [SerializeField] private Projectile _rocket;
    [SerializeField] private Transform _rocketStartPoint;
    [SerializeField] private int _forHitScores;
    [SerializeField] private AudioClip _pvoShootSFX;

    private DamageProcessor _damageProcessor;
    private AudioSource _audioSource;
    private float _starkAttackScreenPointX;

    private bool _fired = false;

    public void Initialize(Vector3 spawnPoint) {
        transform.position = spawnPoint;
        _audioSource = GetComponent<AudioSource>();
        _damageProcessor = GetComponent<DamageProcessor>();
        _damageProcessor.Changed += OnTakeDamage;
        InitizlizeStartAttackScreenPointX();
    }

    private void InitizlizeStartAttackScreenPointX() {
        Camera mainCamera = Camera.main;
        //Определяем точную координату по X крайней правой стороны экрана в соответствии с текущим разрешением экрана
        Vector3 worldViewport = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0));
        //Сохраняем координату X, при которой ПВО начинает стрельбу
        _starkAttackScreenPointX = worldViewport.x + START_ATTACK_SCREEN_X_OFFSET;
    }

    private void OnTakeDamage(float amount) {
        _scoresControll.AddScore(_forHitScores);
        _scoresAddView.Show(_forHitScores);
    }
    public void Reload() {
        _rocket.Show();
        _rocket.transform.position = _rocketStartPoint.position;
        _rocket.transform.rotation = _rocketStartPoint.rotation;
        _fired = false;
    }

    private void Shoot() {
        float _velx = ROCKET_POWER * Mathf.Cos(ROCKET_ANGLE * Mathf.Deg2Rad);
        float _vely = ROCKET_POWER * Mathf.Sin(ROCKET_ANGLE * Mathf.Deg2Rad);
        _rocket.ApplyVelocity(new Vector2(_velx, _vely));
        _audioSource.PlayOneShot(_pvoShootSFX);
        _fired = true;
    }

    private void Update() {
        //Если цель в зоне поражения
        if (transform.position.x < _starkAttackScreenPointX && transform.position.x > 0) {
            //Если ПВО ещё не выстрелило, выстрелить
            if (_fired == false) Shoot();
        }
    }
}