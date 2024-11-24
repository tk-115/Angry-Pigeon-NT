using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class RogatkView : MonoBehaviour {
    private const string IS_HIT = "IsHit";
    private const string IS_DEAD = "IsDead";
    private const string IS_IDLE = "IsIdle";
    private const string IS_ATTACK = "IsAttack";

    [SerializeField] ScoresAddView _scoresAddView;
    [SerializeField] Rogatk _rogatkMain;
    [SerializeField] private List<AudioClip> _shootClips;
    private Animator _animator;
    private AudioSource _audioSource;

    public void Initialize() { 
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void ShootEvent() {
        _audioSource.PlayOneShot(_shootClips[Random.Range(0, _shootClips.Count)]);
        _rogatkMain.RogatkShooter.Shoot();
    }
    public void DisplayScoresAdd(int scores) => _scoresAddView.Show(scores);

    public void DisplayAttack(bool flag) => _animator.SetBool(IS_ATTACK, flag);

    public void DisplayHit(bool flag) => _animator.SetBool(IS_HIT, flag);

    public void DisplayDead(bool flag) => _animator.SetBool(IS_DEAD, flag);

    public void DisplayIDLE(bool flag) => _animator.SetBool(IS_IDLE, flag);
}