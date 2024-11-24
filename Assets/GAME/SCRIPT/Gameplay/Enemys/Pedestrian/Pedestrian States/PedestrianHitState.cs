using UnityEngine;

public class PedestrianHitState : IState {
    private const float HIT_COOLDOWN = .4f;

    private IStateSwicher _swicher;
    private Pedestrian _pedestrianMain;

    private float _timerHitCoolDown;

    public PedestrianHitState(IStateSwicher swicher, Pedestrian pedestrianMain) {
        _swicher = swicher;
        _pedestrianMain = pedestrianMain;
    }

    public void Enter() {
        _pedestrianMain.PedestrianView.DisplayWalk(false);
        _pedestrianMain.PedestrianView.DisplayIDLE(false);
        _pedestrianMain.PedestrianView.DisplayHit(true);
        _timerHitCoolDown = 0;
        _pedestrianMain.PedestrianView.DisplayScoresAdd(_pedestrianMain.ForHitScores);
    }

    public void Exit() => _pedestrianMain.PedestrianView.DisplayHit(false);
    
    public void Update() {
        if (_timerHitCoolDown >= HIT_COOLDOWN) _swicher.SwitchState<PedestrianWalkState>(); else _timerHitCoolDown += Time.deltaTime;
    }
}