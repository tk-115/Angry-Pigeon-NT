using UnityEngine;
using Zenject;

[RequireComponent(typeof(DamageProcessor))]  
public class CityObject : MonoBehaviour {
    [SerializeField] private ScoresAddView _scoreAddView;
    [SerializeField] private int _forHitScores;
    private DamageProcessor _damageProcessor;

    [Inject] private ScoresControll _scoresControll;

    private void Awake() {
        _damageProcessor = GetComponent<DamageProcessor>();
        _damageProcessor.Changed += OnTakeDamage;
        _damageProcessor.Died += OnDie;
    }

    private void OnTakeDamage(float obj) {
        _scoresControll.AddScore(_forHitScores);
        _scoreAddView.Show(_forHitScores);
    }

    private void OnDie() {
        //В будущем можно сделать взрыв, но тогда лучше крутить стейт машину и инициализацию в блоке
        //по аналогии с пешеходом
    }
}