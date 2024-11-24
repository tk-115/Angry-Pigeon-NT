using UnityEngine;

public class PigeonDiedState : IState {
    private PigeonView _pigeonView;
    private Rigidbody2D _rigidbody;

    public PigeonDiedState(PigeonView pigeonView, Rigidbody2D rigidbody) {
        _pigeonView = pigeonView;
        _rigidbody = rigidbody;
    }

    public void Enter() {
        _pigeonView.SetDead(true);
        _rigidbody.gravityScale = 1f;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    public void Exit() {
        _pigeonView.SetDead(false);
        _rigidbody.gravityScale = 0;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void Update() { }
}
