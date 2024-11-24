public class PedestrianDeadState : IState {
    private Pedestrian _pedestrianMain;

    public PedestrianDeadState(Pedestrian pedestrianMain) {
        _pedestrianMain = pedestrianMain;
    }

    public void Enter() {
        _pedestrianMain.PedestrianView.DisplayDead(true);
        _pedestrianMain.PedestrianView.DisplayScoresAdd(_pedestrianMain.ForDeadScores);
    }
    public void Exit() => _pedestrianMain.PedestrianView.DisplayDead(false);
    
    public void Update() { }
}