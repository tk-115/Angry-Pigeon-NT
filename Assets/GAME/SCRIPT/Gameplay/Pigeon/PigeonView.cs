using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PigeonView : MonoBehaviour {
    private const string IS_FLYING = "IsFlying";
    private const string IS_TAKING_DAMAGE = "IsTakingDamage";
    private const string IS_DEAD = "IsDead";

    [SerializeField] private SpriteRenderer _protectionSphereSpriteRenderer;
    [SerializeField] private List<AudioClip> _bombClips;

    private Animator _animator;
    private AudioSource _audioSource;

    public void Initialize() {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>(); 
    }
    public void SetFlying(bool flag) {
        _audioSource.Play();
        _animator.SetBool(IS_FLYING, flag);
    }
    public void SetTakingDamage(bool flag) {
        _audioSource.Stop();
        _animator.SetBool(IS_TAKING_DAMAGE, flag);
    }

    public void SetDead(bool flag) {
        _audioSource.Stop();
        _animator.SetBool(IS_DEAD, flag);
    }

    public void SetIDLE() => _audioSource.Stop();

    public void BombSFXPlay() => _audioSource.PlayOneShot(_bombClips[Random.Range(0, _bombClips.Count)]);

    public void SetActiveProtectionSphere(bool flag) => _protectionSphereSpriteRenderer.enabled = flag;
}
