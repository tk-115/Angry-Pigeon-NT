using UnityEngine;

public abstract class Bonus : MonoBehaviour {

    private bool _isPickedUp = false;

    public bool IsPickedUp => _isPickedUp;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (_isPickedUp == true) return;

        if (collision.TryGetComponent(out IPickable bonusPicker)) {
            Apply(bonusPicker);
            _isPickedUp = true;
        }
    }

    public Sprite HUDIcon;

    public abstract void Apply(IPickable bonusPicker);
    public abstract void Execute(PigeonMain pigeonMain);    //Каждый бонус один хуй может сработать только на голубе иди нахуй
    public abstract void Shutdown();
}
