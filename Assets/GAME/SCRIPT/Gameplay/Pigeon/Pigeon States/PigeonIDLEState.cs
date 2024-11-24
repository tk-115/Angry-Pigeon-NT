
public class PigeonIDLEState : IState {
    private PigeonView _pigeonView;

    public PigeonIDLEState(PigeonView pigeonView) {
        _pigeonView = pigeonView;
    }

    public void Enter() => _pigeonView.SetIDLE();

    public void Exit() { }

    public void Update() { }
}

