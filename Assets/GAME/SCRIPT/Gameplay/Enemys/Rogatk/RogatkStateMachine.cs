using System.Collections.Generic;
using System.Linq;

public class RogatkStateMachine : IStateSwicher {
    private List<IState> _allStates;
    private IState _currentState;

    public RogatkStateMachine(Rogatk _rogatkMain) {
        _allStates = new List<IState>() {
            new RogatkIDLEState(this, _rogatkMain),
            new RogatkHitState(this, _rogatkMain),
            new RogatkDeadState(_rogatkMain),
            new RogatkAttackState(_rogatkMain.RogatkView)
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
