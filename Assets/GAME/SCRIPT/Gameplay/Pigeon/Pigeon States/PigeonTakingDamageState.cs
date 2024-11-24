using UnityEngine;

public class PigeonTakingDamageState : IState
{
    private const float TAKING_DAMAGE_COOLDOWN = .2f;

    private IStateSwicher _stateSwither;
    private PigeonView _pigeonView;

    private float _timerCoolDown;

    public PigeonTakingDamageState(IStateSwicher stateSwither, PigeonView pigeonView) {
        _stateSwither = stateSwither;
        _pigeonView = pigeonView;
    }

    public void Enter() {
        _pigeonView.SetTakingDamage(true);
        _timerCoolDown = 0;
    }

    public void Exit() {
        _pigeonView.SetTakingDamage(false);
    }

    public void Update() {
        if (_timerCoolDown <= TAKING_DAMAGE_COOLDOWN) _timerCoolDown += Time.deltaTime; else _stateSwither.SwitchState<PigeonFlyingState>();
    }
}
