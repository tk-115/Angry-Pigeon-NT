using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PigeonStateMachine : IStateSwicher {
    private List<IState> _allStates;
    private IState _currentState;

    public PigeonStateMachine(PigeonMain pigeon, GameplayView gameplayView, Rigidbody2D rigidbody) {

        _allStates = new List<IState>() {
            new PigeonIDLEState(pigeon.View),
            new PigeonFlyingState(this, pigeon, gameplayView),
            new PigeonTakingDamageState(this, pigeon.View),
            new PigeonDiedState(this, pigeon.View, rigidbody)
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

    public bool CurrentStateIs<T>() where T : IState {
        if (_currentState is T) return true; else return false;
    }

    public void Update() => _currentState.Update();
}
