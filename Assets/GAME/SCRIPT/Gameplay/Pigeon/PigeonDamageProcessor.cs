using UnityEngine;
using Zenject;

public class PigeonDamageProcessor : DamageProcessor {
    [Inject] private GameplayView _gameplayView;
    
    public bool IsInvulnerable = false;

    public override void TakeDamage(float amount) {
        if (IsInvulnerable == false) {
            base.TakeDamage(amount);
            _gameplayView.SetHealthBarValue(_health);
        }
    }

    public override void Heal() {
        base.Heal();
        _gameplayView.SetHealthBarValue(_health);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        //Так как урон может быть нанесен объектами окружения, все они должны иметь 
        //Tag, отличающийся от IgnoreCollision, а projectile, которые наносят урон,
        //должны иметь Tag = IgnoreCollision для нанесения уровна по схеме IDamageble
        if (collision.transform.tag != "IgnoreCollision") TakeDamage(1f);
    }
}