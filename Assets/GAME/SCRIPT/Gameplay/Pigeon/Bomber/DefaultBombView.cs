using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class DefaultBombView : MonoBehaviour {
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;

    [SerializeField] private List<AudioClip> _collideClips;

    private Color _colorDefault = new Color(255, 255, 255, 255);
    private Color _colorTransparent = new Color(255, 255, 255, 0);

    private void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void DisplayCollide() {
        _spriteRenderer.color = _colorDefault;
        _animator.Play("BombExplosion");
        _audioSource.PlayOneShot(_collideClips[Random.Range(0, _collideClips.Count)]);
    }

    public void DisplayEmpty() => _spriteRenderer.color = _colorTransparent;

    public void DisplayNormal() {
        _spriteRenderer.color = _colorDefault;
        _animator.Play("BombIDLE");
    }
}