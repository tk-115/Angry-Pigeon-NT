using UnityEngine;
using Zenject;

public class BonusCoin : Bonus {
    [SerializeField] private BonusCoinView _bonusView;

    [Inject] private ScoresControll _scoresControll;

    public override void Apply(IPickable bonusPicker) => bonusPicker.Apply(this);

    public void Initialize() {
        _bonusView.Initialize();
        _bonusView.DisplayNormal();
    }

    //Запуск логики бонуса
    public override void Execute(PigeonMain pigeonMain) {
        transform.SetParent(null);
        _bonusView.DisplayHide();
        _scoresControll.AddCoin();
        Shutdown();
    }

    //Завершение логики бонуса
    public override void Shutdown() => Destroy(gameObject);
}