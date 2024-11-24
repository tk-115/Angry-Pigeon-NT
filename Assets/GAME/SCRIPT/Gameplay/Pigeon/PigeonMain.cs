using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PigeonBonusControl))]
[RequireComponent (typeof(PigeonDamageProcessor))]
public class PigeonMain : MonoBehaviour {
    [SerializeField] private PigeonView _pigeonView;

    private Rigidbody2D _rigidBody;
    private PigeonStateMachine _stateMachine;
    private PigeonDamageProcessor _damageProcessor;
    private PigeonBonusControl _bonusControl;
    private PigeonBomber _bomber;

    private Transform _spawnPoint;

    public PigeonView View => _pigeonView;
    public Rigidbody2D Rigidbody => _rigidBody;
    public PigeonStateMachine StateMachine => _stateMachine;
    public PigeonDamageProcessor DamageProcessor => _damageProcessor;
    public PigeonBomber Bomber => _bomber;
    public PigeonBonusControl BonusControl => _bonusControl;

    private void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
        _bomber = GetComponent<PigeonBomber>();
        _bonusControl = GetComponent<PigeonBonusControl>();
        _damageProcessor = GetComponent<PigeonDamageProcessor>();
        _damageProcessor.Changed += OnTakingDamage;
        _damageProcessor.Died += OnDied;
    }

    public void Initialize(GameplayView gameplayView, Transform spawnPoint) {
        _spawnPoint = spawnPoint;
        _pigeonView.Initialize();
        _bonusControl.Initialize(this);
        _stateMachine = new PigeonStateMachine(this, gameplayView, _rigidBody);
    }

    public void Respawn() {
        transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
        _damageProcessor.Heal();
        _stateMachine.SwitchState<PigeonFlyingState>();
    }
    
    private void Update() { if (_stateMachine != null) _stateMachine.Update(); }

    private void OnTakingDamage(float amount) => _stateMachine.SwitchState<PigeonTakingDamageState>();

    private void OnDied() {
        //завершаем работу текущего бонуса
        _bonusControl.TerminateCurrentBonus();
        //Переключаемя в состояние смерти
        _stateMachine.SwitchState<PigeonDiedState>();
    }
}
