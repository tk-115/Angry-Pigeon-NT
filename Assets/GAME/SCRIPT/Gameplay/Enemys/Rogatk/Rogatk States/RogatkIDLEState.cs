public class RogatkIDLEState : IState {
    IStateSwicher _stateSwicher;
    private Rogatk _rogatkMain;

    public RogatkIDLEState(IStateSwicher stateSwicher, Rogatk rogatMain) {
        _stateSwicher = stateSwicher;
        _rogatkMain = rogatMain;
    }

    public void Enter() {
        _rogatkMain.RogatkView.DisplayIDLE(true);
        _rogatkMain.RogatkView.DisplayAttack(false);
        _rogatkMain.RogatkView.DisplayHit(false);
        _rogatkMain.RogatkView.DisplayDead(false);
    }
    
    public void Exit() => _rogatkMain.RogatkView.DisplayIDLE(false);

    public void Update() {
        //Рогатчик начинает стрельбу при достижении видимости игрока
        if (_rogatkMain.transform.position.x < _rogatkMain.RogatkShooter.StarkAttackScreenPointX && _rogatkMain.transform.position.x > 0) {
            //И если у него есть камни
            if (_rogatkMain.RogatkShooter.HasStones()) {
                _stateSwicher.SwitchState<RogatkAttackState>();
            }
        }
    }
}