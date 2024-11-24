using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class BonusCoinView : MonoBehaviour {
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    public void Initialize() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void DisplayNormal() => _animator.Play("BonusCoinRotation");
    
    public void DisplayHide() {
        _animator.Play("IDLE");
        _spriteRenderer.enabled = false;
    }
}