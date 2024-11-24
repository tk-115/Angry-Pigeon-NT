using System;
using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(DamageProcessor))]  
public class Vorona : MonoBehaviour {
    private const int END_FLY_POSITION_X_OFFSET = 3;
    private const float FLY_FORCE = -.25f;

    public Action OnVoronaFlyedEvent;

    [Inject] private ScoresControll _scoresControll;

    [SerializeField] private VoronaView _voronaView;
    [SerializeField] private int _scoresForHit;

    private DamageProcessor _damageProcessor;
    private Rigidbody2D _rigidbody;
    private Coroutine _flyCoroutine;
    private float _endFlyPositionX;

    private void Awake() {
        _damageProcessor = GetComponent<DamageProcessor>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _damageProcessor.Changed += OnTakeDamage;
        InitizlizeEndFlyPositionX();
    }

    private void InitizlizeEndFlyPositionX() {
        Camera mainCamera = Camera.main;
        //Определяем точную координату по X крайней левой стороны экрана в соответствии с текущим разрешением экрана
        Vector3 worldViewport = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        //Сохраняем координату X с оффсетом
        _endFlyPositionX = worldViewport.x - END_FLY_POSITION_X_OFFSET;
    }

    IEnumerator FlyCycle() {
        while (transform.position.x > _endFlyPositionX) {
            _rigidbody.AddForce(new Vector2(FLY_FORCE, 0f), ForceMode2D.Impulse);
            yield return new WaitForFixedUpdate();
        }
        _voronaView.DisplayIDLE();
        OnVoronaFlyedEvent?.Invoke();
    }

    public void StartFly(Vector3 startPosition) {
        transform.position = startPosition;
        _voronaView.DisplayFly();
        if (_flyCoroutine != null) StopCoroutine(_flyCoroutine);
        _flyCoroutine = StartCoroutine(FlyCycle());
    }

    private void OnTakeDamage(float amount) {
        _scoresControll.AddScore(_scoresForHit);
        _voronaView.DisplayAddScores(_scoresForHit);
    }
}