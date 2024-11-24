using System;
using System.Collections;
using UnityEngine;

public class HomingBomb : Bomb {
    private const float DISABLE_AFTER = 3f;
    private const float TARGET_SCAN_POSITION_X_OFFSET = 9;
    private const float CORRECT_ANGLE_AFTER_TARGET_NAVIGATION = 180;

    [SerializeField] private DefaultBombView _bombView;
    [SerializeField, Range(0f, 1f)] private float _damage;
    [SerializeField] private float _homingRadius;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _homingAfterSec;
    [SerializeField, Range(0.3f, 1f)] private float _speed;

    private Rigidbody2D _rigidbody;
    private bool _isCollided = false;
    private Transform _parent;

    private Coroutine _deactivateBombCoroutine;
    private Coroutine _homingStartCoroutine;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void InitializeParent(Transform parent) => _parent = parent;

    public override void Drop() {
        transform.parent = null;
        _bombView.DisplayNormal();
        _rigidbody.gravityScale = 1f;

        if (_homingStartCoroutine != null) StopCoroutine(_homingStartCoroutine);
        _homingStartCoroutine = StartCoroutine(HomingCoroutine());

        if (_deactivateBombCoroutine != null) StopCoroutine(_deactivateBombCoroutine);
        _deactivateBombCoroutine = StartCoroutine(DeactivateCoroutine());
    }

    public void Resett() {
        _isCollided = false;
        _bombView.DisplayEmpty();
        _rigidbody.gravityScale = 0f;
    }

    private IEnumerator HomingCoroutine() {
        yield return new WaitForSeconds(_homingAfterSec);
        //В радиусе берем ближайшего противника и двигаем ракету в него до коллизии

        Vector3 modPosition = new Vector3(transform.position.x + TARGET_SCAN_POSITION_X_OFFSET, transform.position.y, transform.position.z);
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(modPosition, _homingRadius, _layerMask);

        if (hitColliders.Length > 0) {
            float minDistance = Vector3.Distance(modPosition, hitColliders[0].transform.position);
            int colId = 0;
            for (int i = 0; i < hitColliders.Length; i++) {
                float distanceToTarget = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                if (distanceToTarget < minDistance) {
                    minDistance = distanceToTarget;
                    colId = i;
                }
            }
            float timer = 0;
            Vector3 startPosition = transform.position;

            float angle = Mathf.Atan2(hitColliders[colId].transform.position.y, hitColliders[colId].transform.position.x) * Mathf.Rad2Deg;
            angle += CORRECT_ANGLE_AFTER_TARGET_NAVIGATION;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

            while (timer < _speed) {
                transform.position = Vector3.Lerp(startPosition, hitColliders[colId].transform.position, timer / _speed);
                timer += Time.deltaTime;
                yield return null;
            }
        }
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

        //stop homing - bomb exploded already
        if (_homingStartCoroutine != null) StopCoroutine(_homingStartCoroutine);

        //При касании любого объекта, бомба взрывается и в радиусе береутся все, кто может быть поврежден
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _layerMask);
        foreach (var hitCollider in hitColliders) {
            if (hitCollider.transform.TryGetComponent(out IDamageble damageble)) damageble.TakeDamage(_damage);
        }

        _isCollided = true;
        _bombView.DisplayCollide();
    }
}