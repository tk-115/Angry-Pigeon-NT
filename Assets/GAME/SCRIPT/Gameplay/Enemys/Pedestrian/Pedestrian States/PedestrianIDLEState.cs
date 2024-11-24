public class PedestrianIDLEState : IState {
    private PedestrianView _pedestrianView;

    public PedestrianIDLEState(PedestrianView pedestrianView) {
        _pedestrianView = pedestrianView;
    }

    public void Enter() => _pedestrianView.DisplayIDLE(true);
    
    public void Exit() => _pedestrianView.DisplayIDLE(false);
    
    public void Update() { }
}