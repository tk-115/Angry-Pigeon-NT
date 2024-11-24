using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TripleBombUnit : MonoBehaviour {
    private const float DISABLE_AFTER = 3f;

    [SerializeField] private float _power, _angle;
    [SerializeField] private DefaultBombView _bombView;
    [SerializeField, Range(0f, 1f)] private float _damage;

    private Rigidbody2D _rigidbody;
    private Transform _parent;
    private bool _isCollided = false;

    private Coroutine _deactivateBombsCoroutine;

    public Rigidbody2D Rigidbody => _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDefaultParent(Transform parent) => _parent = parent;

    public void Resett() {
        _isCollided = false;
        _bombView.DisplayEmpty();
        _rigidbody.gravityScale = 0f;
    }

    public void Drop() {
        transform.parent = null;
        _bombView.DisplayNormal();
        _rigidbody.gravityScale = 1f;

        float _velx = _power * Mathf.Cos(_angle * Mathf.Deg2Rad);
        float _vely = _power * Mathf.Sin(_angle * Mathf.Deg2Rad);
        _rigidbody.velocity = new Vector2(_velx, _vely);

        if (_deactivateBombsCoroutine != null) StopCoroutine(_deactivateBombsCoroutine);
        _deactivateBombsCoroutine = StartCoroutine(DeactivateCoroutine());
    }

    private IEnumerator DeactivateCoroutine() {
        yield return new WaitForSeconds(DISABLE_AFTER);
        DeactivateBomb();
    }

    private void DeactivateBomb() {
        Resett();
        gameObject.SetActive(false);
        transform.parent = _parent; //для порядка на сцене
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (_isCollided == true) return;

        if (collision.transform.TryGetComponent(out IDamageble damageble)) damageble.TakeDamage(_damage);
        
        _isCollided = true;
        _bombView.DisplayCollide();
    }
}