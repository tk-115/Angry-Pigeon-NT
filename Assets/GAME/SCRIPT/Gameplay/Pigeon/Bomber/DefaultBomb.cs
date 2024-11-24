using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DefaultBomb : Bomb {
    private const float DISABLE_AFTER = 3f;

    [SerializeField] private DefaultBombView _bombView;
    [SerializeField, Range(0f, 1f)] private float _damage;
    private Rigidbody2D _rigidbody;
    private bool _isCollided = false;
    private Transform _parent;

    private Coroutine _deactivateBombCoroutine;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void InitializeParent(Transform parent) => _parent = parent;

    public override void Drop() {
        //можно переопределять как угодно - данная еденица дропается и может разбрасывать доп частицы куда угодно 
        //дефолтная просто становится гравитационной и все 
        transform.parent = null;
        _bombView.DisplayNormal();
        _rigidbody.gravityScale = 1f;
        if (_deactivateBombCoroutine != null) StopCoroutine(_deactivateBombCoroutine);
        _deactivateBombCoroutine = StartCoroutine(DeactivateCoroutine());
    }

    public void Resett() {
        _isCollided = false;
        _bombView.DisplayEmpty();
        _rigidbody.gravityScale = 0f;
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