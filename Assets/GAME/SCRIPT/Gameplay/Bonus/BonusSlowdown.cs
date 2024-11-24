using UnityEngine;
using Zenject;

public class BonusSlowdown : Bonus {
    [SerializeField] private SpriteRenderer _bonusView;

    private PigeonMain _pigeonMain;

    [Inject] private GameplayView _gameplayView;
    [Inject] private LevelGenerator _levelGenerator;

    private Coroutine _bonusCoroutine;

    public override void Apply(IPickable bonusPicker) => bonusPicker.Apply(this);

    //Запуск логики бонуса
    public override void Execute(PigeonMain pigeonMain) {
        //Вытаскиваем бонус из блока 
        transform.SetParent(null);
        //Отключаем его View
        _bonusView.enabled = false;
        _pigeonMain = pigeonMain;
        //sfx
        _gameplayView.NewBonusSFXPlay();
        //Запускаем логику работы бонуса
        _levelGenerator.BonusSlowDownLogic();
    }

    //Завершение логики бонуса
    public override void Shutdown() => Destroy(gameObject);
}