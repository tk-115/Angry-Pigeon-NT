using System;
using UnityEngine;

public class DamageProcessor : MonoBehaviour, IDamageble, IHealeble {
    public event Action<float> Changed;
    public event Action Died;
    public event Action Healed;

    protected float _health;
    [SerializeField] protected float _maxHealth;

    private void Awake() {
        _health = _maxHealth;
    }

    public virtual void TakeDamage(float amount) {
        if (_health <= 0) return;

        _health -= amount;

        if (_health <= 0) Died?.Invoke(); else Changed?.Invoke(_health);
    }

    public virtual void Heal() {
        _health = _maxHealth;
        Healed?.Invoke();
    }
}
