using UnityEngine;
using Zenject;

[RequireComponent(typeof(DamageProcessor))]
public class Pedestrian : MonoBehaviour {
    [Inject] private ScoresControll _scoresControll;

    [SerializeField] private PedestrianView _pedestrianView;

    [SerializeField] private int _forHitScores;
    [SerializeField] private int _forDeadScores;

    private Transform[] _navPoints;
    private DamageProcessor _damageProcessor;
    private PedestrianStateMachine _pedestrianStateMachine;

    public DamageProcessor DamageProcessor => _damageProcessor;
    public PedestrianView PedestrianView => _pedestrianView;
    public Transform[] NavPoints => _navPoints;
    public int ForHitScores => _forHitScores;
    public int ForDeadScores => _forDeadScores;

    public void Initialize(Transform[] points) {
        _damageProcessor = GetComponent<DamageProcessor>();
        _damageProcessor.Changed += OnTakeDamage;
        _damageProcessor.Died += OnDie;
        _navPoints = points;
        _pedestrianStateMachine = new PedestrianStateMachine(this);
    }

    public void Update() => _pedestrianStateMachine.Update();

    public void SetPosition(Vector3 position) => transform.position = position;

    public void StartWalk() => _pedestrianStateMachine.SwitchState<PedestrianWalkState>();

    private void OnTakeDamage(float amount) {
        _scoresControll.AddScore(ForHitScores);
        _pedestrianStateMachine.SwitchState<PedestrianHitState>();
    }
    private void OnDie() {
        _scoresControll.AddScore(ForDeadScores);
        _pedestrianStateMachine.SwitchState<PedestrianDeadState>();
    }
    
}
