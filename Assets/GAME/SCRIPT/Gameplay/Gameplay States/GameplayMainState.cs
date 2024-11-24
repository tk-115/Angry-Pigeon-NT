public class GameplayMainState : IState {

    private IStateSwicher _stateSwicher;
    private PigeonMain _pigeonMain;
    private GameplayView _gameplayView;

    public GameplayMainState(IStateSwicher stateSwicher, PigeonMain pigeonMain, GameplayView gameplayView) {
        _stateSwicher = stateSwicher;
        _pigeonMain = pigeonMain;
        _gameplayView = gameplayView;
    }

    public void Enter() {
        _pigeonMain.StateMachine.SwitchState<PigeonFlyingState>();
        _gameplayView.OnButtonPauseClickEvent += OnButtonPlayClicked;
    }

    public void Exit() => _gameplayView.OnButtonPauseClickEvent -= OnButtonPlayClicked;

    public void Update() { }

    private void OnButtonPlayClicked() => _stateSwicher.SwitchState<PauseState>();
    
}