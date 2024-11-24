using System.Collections.Generic;
using System.Linq;

public class PedestrianStateMachine : IStateSwicher {

    private List<IState> _allStates;
    private IState _currentState;

    public PedestrianStateMachine(Pedestrian _pedestrianMain) {
        _allStates = new List<IState>() {
            new PedestrianIDLEState(_pedestrianMain.PedestrianView),
            new PedestrianWalkState(_pedestrianMain),
            new PedestrianHitState(this, _pedestrianMain),
            new PedestrianDeadState(_pedestrianMain)
        };

        _currentState = _allStates[0];
        _currentState.Enter();
    }

    public void SwitchState<T>() where T : IState {
        IState state = _allStates.FirstOrDefault(state => state is T);

        if (state == null) throw new System.ArgumentOutOfRangeException("Required state not finded");

        _currentState.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    public void Update() => _currentState.Update();
}