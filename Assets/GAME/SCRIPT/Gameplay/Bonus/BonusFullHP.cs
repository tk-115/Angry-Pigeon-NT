using UnityEngine;
using Zenject;

public class BonusFullHP : Bonus {
    [SerializeField] private SpriteRenderer _bonusView;

    [Inject] private GameplayView _gameplayView;

    public override void Apply(IPickable bonusPicker) => bonusPicker.Apply(this);

    //Запуск логики бонуса
    public override void Execute(PigeonMain pigeonMain) {
        transform.SetParent(null);
        _bonusView.enabled = false;
        _gameplayView.NewBonusSFXPlay();
        pigeonMain.DamageProcessor.Heal();
        Shutdown();
    }

    //Завершение логики бонуса
    public override void Shutdown() => Destroy(gameObject);
}