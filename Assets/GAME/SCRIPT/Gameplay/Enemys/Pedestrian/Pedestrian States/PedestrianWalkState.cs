using UnityEngine;

public class PedestrianWalkState : IState {
    private const float SPEED = 1f;

    private Pedestrian _pedestrianMain;

    public PedestrianWalkState(Pedestrian pedestrianMain) {
        _pedestrianMain = pedestrianMain;
    }

    public void Enter() {
        _pedestrianMain.PedestrianView.DisplayIDLE(false);
        _pedestrianMain.PedestrianView.DisplayDead(false);
        _pedestrianMain.PedestrianView.DisplayHit(false);
        _pedestrianMain.PedestrianView.DisplayWalk(true);
    }

    public void Exit() {
        _pedestrianMain.PedestrianView.DisplayWalk(false);
    }

    public void Update() {
        float _move = Time.deltaTime * SPEED;
        
        //Если пешеход зеркально не отображен
        if (_pedestrianMain.PedestrianView.SpriteFlipX == false) {
            //Если пешеход дошел к финальной точке, зеркально отображаем его
            if (_pedestrianMain.transform.position.x >= _pedestrianMain.NavPoints[1].position.x)
                _pedestrianMain.PedestrianView.SetSpriteFlip(true);
        } else {
            //Движение влево
            _move = 0 - _move;
            //Если пешеход дошел к стартовой точке, зеркально перестаем отображать его
            if (_pedestrianMain.transform.position.x <= _pedestrianMain.NavPoints[0].position.x)
                _pedestrianMain.PedestrianView.SetSpriteFlip(false);
        }
        
        //Двигаем пешехода согласно его направлению
        _pedestrianMain.transform.Translate(_move, 0, 0);
    }
}