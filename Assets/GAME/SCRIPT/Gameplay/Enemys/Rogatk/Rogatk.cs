using UnityEngine;
using Zenject;

[RequireComponent(typeof(DamageProcessor))]
[RequireComponent (typeof(RogatkShooter))]
public class Rogatk : MonoBehaviour {
    [Inject] private ScoresControll _scoresControll;

    [SerializeField] private RogatkView _rogatkView;

    [SerializeField] private int _forHitScores;
    [SerializeField] private int _forDeadScores;

    private DamageProcessor _damageProcessor;
    private RogatkShooter _rogatkShooter;
    private RogatkStateMachine _rogatkStateMachine;

    public RogatkView RogatkView => _rogatkView;
    public RogatkShooter RogatkShooter => _rogatkShooter;
    public int ForHitScores => _forHitScores;
    public int ForDeadScores => _forDeadScores;
    public DamageProcessor DamageProcessor => _damageProcessor;  

    public void Initialize() {
        _rogatkView.Initialize();
        _damageProcessor = GetComponent<DamageProcessor>();
        _damageProcessor.Changed += OnTakeDamage;
        _damageProcessor.Died += OnDie;
        _rogatkShooter = GetComponent<RogatkShooter>();
        _rogatkShooter.Initialize();
        _rogatkStateMachine = new RogatkStateMachine(this);
    }

    public void SetIDLE() => _rogatkStateMachine.SwitchState<RogatkIDLEState>();

    private void Update() => _rogatkStateMachine.Update();

    private void OnTakeDamage(float amount) {
        _scoresControll.AddScore(ForHitScores);
        _rogatkStateMachine.SwitchState<RogatkHitState>();
    }

    private void OnDie() {
        Debug.Log(name + " OnDie execute");
        _scoresControll.AddScore(ForDeadScores);
        _rogatkStateMachine.SwitchState<RogatkDeadState>();
    }
}
