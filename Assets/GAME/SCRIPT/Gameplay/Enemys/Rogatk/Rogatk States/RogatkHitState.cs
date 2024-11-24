using System;
using UnityEngine;

public class RogatkHitState : IState {
    private const float HIT_COOLDOWN = .4f;

    private IStateSwicher _swicher;
    private Rogatk _rogatkMain;

    private float _timerHitCoolDown;

    public RogatkHitState(IStateSwicher swicher, Rogatk rogatkMain) {
        _swicher = swicher;
        _rogatkMain = rogatkMain;
    }

    public void Enter() {
        _rogatkMain.RogatkView.DisplayHit(true);
        _rogatkMain.RogatkView.DisplayAttack(false);
        _rogatkMain.RogatkView.DisplayIDLE(false);
        _timerHitCoolDown = 0;
        _rogatkMain.RogatkView.DisplayScoresAdd(_rogatkMain.ForHitScores);
    }

    public void Exit() {
        _rogatkMain.RogatkView.DisplayHit(false);
    }

    public void Update() {
        if (_timerHitCoolDown >= HIT_COOLDOWN) _swicher.SwitchState<RogatkIDLEState>(); else _timerHitCoolDown += Time.deltaTime;
    }
}