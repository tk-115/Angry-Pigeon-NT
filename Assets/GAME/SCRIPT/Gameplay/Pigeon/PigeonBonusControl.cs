using UnityEngine;

public class PigeonBonusControl : MonoBehaviour, IPickable {
    private Bonus _currentBonus;
    public Bonus CurrentBonus => _currentBonus;

    private PigeonMain _pigeonMain;

    public void Initialize(PigeonMain pigeonMain) { 
        _pigeonMain = pigeonMain;
    }

    public void Apply(Bonus bonus) {
        if (bonus is BonusCoin) {
            bonus.Execute(_pigeonMain);
        } else {
            TerminateCurrentBonus();
            _currentBonus = bonus;
            _currentBonus.Execute(_pigeonMain);
        }
    }

    public void TerminateCurrentBonus() {
        if (CurrentBonus != null) CurrentBonus.Shutdown();
    }
}