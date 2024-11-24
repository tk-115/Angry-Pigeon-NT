using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour {
    private const int LIFE_TIME = 5;
    [SerializeField] private float _damage;
    [SerializeField] private List<AudioClip> _hitSFXes;

    private AudioSource _audioSource;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private bool _hited = false;

    private Coroutine _LifeTimeControlCoroutine;

    private void Awake() { 
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _rigidbody.gravityScale = 0f;
        _collider.isTrigger = true;
    }

    public void Reinstall() {
        Hide();
        _rigidbody.gravityScale = 0f;
        _collider.isTrigger = true;
        _hited = false;
    }

    public void Hide() => _spriteRenderer.enabled = false;

    public void Show() {
        if (_LifeTimeControlCoroutine != null) StopCoroutine(_LifeTimeControlCoroutine);
        _spriteRenderer.enabled = true;
    }
    public void ApplyVelocity(Vector2 velocity) {
        _rigidbody.gravityScale = 1f;
        _collider.isTrigger = false;
        _rigidbody.velocity = velocity;

        if (_LifeTimeControlCoroutine != null) StopCoroutine(_LifeTimeControlCoroutine);
        _LifeTimeControlCoroutine = StartCoroutine(LifeTimeControlCoroutine());
    }

    private IEnumerator LifeTimeControlCoroutine() { 
        yield return new WaitForSeconds(LIFE_TIME);
        Reinstall();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (_hited) return;

        if (collision.transform.TryGetComponent(out IDamageble damageble)) damageble.TakeDamage(_damage);
        
        _audioSource.PlayOneShot(_hitSFXes[Random.Range(0, _hitSFXes.Count)]);
        _hited = true;
    }
}