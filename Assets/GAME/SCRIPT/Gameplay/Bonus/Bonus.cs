using UnityEngine;

public abstract class Bonus : MonoBehaviour {
    public bool IsPickedUp => _isPickedUp;
    public Sprite HUDIcon;
    public abstract void Apply(IPickable bonusPicker);
    public abstract void Execute(PigeonMain pigeonMain);    
    public abstract void Shutdown();

    private bool _isPickedUp = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (_isPickedUp == true) return;

        if (collision.TryGetComponent(out IPickable bonusPicker)) {
            Apply(bonusPicker);
            _isPickedUp = true;
        }
    }
}