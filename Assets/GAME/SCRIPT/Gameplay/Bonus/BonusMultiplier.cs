using System.Collections;
using UnityEngine;
using Zenject;

public class BonusMultiplier : Bonus {
    private int _durationOfWork = 15;
    [SerializeField] private SpriteRenderer _bonusView;

    private PigeonMain _pigeonMain;

    [Inject] private GameplayView _gameplayView;
    [Inject] private ScoresControll _scoresControll;

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
        if (_bonusCoroutine != null) StopCoroutine(_bonusCoroutine);
        _bonusCoroutine = StartCoroutine(BonusCoroutine());
    }

    private IEnumerator BonusCoroutine() {
        //применить привелегии бонуса
        _gameplayView.SetCurrentBonusIcon(HUDIcon);
        _scoresControll.SetMultiplierActive();
        //дождаться окончания времени работы бонуса
        yield return new WaitForSeconds(_durationOfWork);
        //завершить его работу
        Shutdown();
    }

    //Завершение логики бонуса
    public override void Shutdown() {
        if (_bonusCoroutine != null) StopCoroutine(_bonusCoroutine);
        //Убрать привелегии бонуса
        _scoresControll.SetMultiplierDefault();
        _gameplayView.ClearCurrentBonusIcon();

        //Удаляем бонус со сцены
        Destroy(gameObject);
    }
}

