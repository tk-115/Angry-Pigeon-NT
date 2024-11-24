using UnityEngine;

[RequireComponent (typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class VoronaView : MonoBehaviour {
    private Animator _animator;
    private AudioSource _audioSource;

    [SerializeField] private ScoresAddView _scoresAddView;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void DisplayAddScores(int scores) => _scoresAddView.Show(scores);

    public void DisplayIDLE() {
        _animator.Play("IDLE");
        _audioSource.Stop();
    }

    public void DisplayFly() {
        _animator.Play("Fly");
        _audioSource.Play();
    }
}