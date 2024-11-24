using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]    
public class ScoresAddView : MonoBehaviour {
    [SerializeField] private TextMeshPro _text;
    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _text.enabled = false;
    }

    public void Show(int score) {
        _text.enabled = true;
        _text.text = score.ToString();
        _animator.Play("Show");
    }
}