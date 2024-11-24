using System.Collections;
using UnityEngine;
using Zenject;

public class BonusProtection : Bonus {
    private int _durationOfWork = 12;
    [SerializeField] private SpriteRenderer _bonusView;
    private PigeonMain _pigeonMain;
    private Coroutine _bonusCoroutine;

    [Inject] private GameplayView _gameplayView;

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
        _pigeonMain.DamageProcessor.IsInvulnerable = true;
        _gameplayView.SetCurrentBonusIcon(HUDIcon);
        _pigeonMain.View.SetActiveProtectionSphere(true);
        //дождаться окончания времени работы бонуса
        yield return new WaitForSeconds(_durationOfWork);
        //завершить его работу
        Shutdown();
    }

    //Завершение логики бонуса
    public override void Shutdown() {
        if (_bonusCoroutine != null) StopCoroutine(_bonusCoroutine);
        //Убрать привелегии бонуса
        _pigeonMain.DamageProcessor.IsInvulnerable = false;
        _gameplayView.ClearCurrentBonusIcon();
        _pigeonMain.View.SetActiveProtectionSphere(false);
        Destroy(gameObject);
    }
}