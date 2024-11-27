public class RogatkDeadState : IState {
    private Rogatk _rogatkMain;

    public RogatkDeadState(Rogatk rogatkMain) {
        _rogatkMain = rogatkMain;
    }

    public void Enter() {
        _rogatkMain.RogatkView.DisplayAttack(false);
        _rogatkMain.RogatkView.DisplayIDLE(false);
        _rogatkMain.RogatkView.DisplayDead(true);
        _rogatkMain.RogatkView.DisplayScoresAdd(_rogatkMain.ForDeadScores);
    }

    public void Exit() => _rogatkMain.RogatkView.DisplayDead(false);
    
    public void Update() { }
}