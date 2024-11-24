using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PedestrianView : MonoBehaviour {
    private const string IS_HIT = "IsHit";
    private const string IS_DEAD = "IsDead";
    private const string IS_WALK = "IsWalk";
    private const string IS_IDLE = "IsIdle";

    [SerializeField] ScoresAddView _scoresAddView;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    public bool SpriteFlipX => _spriteRenderer.flipX;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DisplayScoresAdd(int scores) => _scoresAddView.Show(scores);

    public void SetSpriteFlip(bool flag) => _spriteRenderer.flipX = flag;

    public void DisplayWalk(bool flag) => _animator.SetBool(IS_WALK, flag);

    public void DisplayHit(bool flag) => _animator.SetBool(IS_HIT, flag);

    public void DisplayDead(bool flag) => _animator.SetBool(IS_DEAD, flag);

    public void DisplayIDLE(bool flag) => _animator.SetBool(IS_IDLE, flag);
}
