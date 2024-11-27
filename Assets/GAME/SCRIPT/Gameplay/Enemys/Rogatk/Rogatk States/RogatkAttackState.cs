public class RogatkAttackState : IState {

    private RogatkView _rogatkView;

    public RogatkAttackState(RogatkView rogatkView) {
        _rogatkView = rogatkView;
    }

    public void Enter() {
        _rogatkView.DisplayIDLE(false);
        _rogatkView.DisplayAttack(true);
    }

    public void Exit() => _rogatkView.DisplayAttack(false);
    
    public void Update() { }
}

