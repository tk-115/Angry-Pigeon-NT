using System;
using UnityEngine;

public class PigeonFlyingState : IState {
    private const float FLIGHT_RESPONSIVENESS = .1f;

    public event Action OnButtonControllUPClickEvent;
    public event Action OnButtonControllDOWNClickEvent;
    public event Action OnButtonControllBOMBClickEvent;

    private PigeonMain _pigeonMain;
    private GameplayView _gameplayView;

    private float _inputFlyControl;
    private float _timerBombCoolDown;

    public PigeonFlyingState(PigeonMain pigeonMain, GameplayView gameplayView) {
        _pigeonMain = pigeonMain;
        _gameplayView = gameplayView;
    }

    public void Enter() {
        _pigeonMain.View.SetFlying(true);
        _timerBombCoolDown = 0;

        //подписаться на кнопки управления 
        _gameplayView.OnButtonControllDeclickEvent += OnButtonControllDeclicked;
        _gameplayView.OnButtonControllDownClickEvent += OnButtonControllDOWNClicked;
        _gameplayView.OnButtonControllUPClickEvent += OnButtonControllUPClicked;
        _gameplayView.OnButtonControllBombClickEvent += OnButtonControllBOMBClicked;

        _inputFlyControl = 0;
    }

    private void OnButtonControllBOMBClicked() {
        if (_timerBombCoolDown >= _pigeonMain.Bomber.CoolDown) {
            _timerBombCoolDown = 0;
            _pigeonMain.Bomber.DropBomb();
        }
    }

    private void OnButtonControllDOWNClicked() => _inputFlyControl = -1;
    
    private void OnButtonControllUPClicked() => _inputFlyControl = 1;
    
    private void OnButtonControllDeclicked() => _inputFlyControl = 0; 
    
    public void Exit() {
        _pigeonMain.View.SetFlying(false);

        //отписаться от кнопок управления
        _gameplayView.OnButtonControllDeclickEvent -= OnButtonControllDeclicked;
        _gameplayView.OnButtonControllDownClickEvent -= OnButtonControllDOWNClicked;
        _gameplayView.OnButtonControllUPClickEvent -= OnButtonControllUPClicked;
        _gameplayView.OnButtonControllBombClickEvent -= OnButtonControllBOMBClicked;
    }

    public void Update() {
        //pc controll for debug
        if (Input.GetKeyDown(KeyCode.UpArrow)) OnButtonControllUPClicked();
        if (Input.GetKeyDown(KeyCode.DownArrow)) OnButtonControllDOWNClicked();
        if (Input.GetKeyDown(KeyCode.Space)) OnButtonControllBOMBClicked();
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) _inputFlyControl = 0;

        if (_inputFlyControl < 0 || _inputFlyControl > 0)
            _pigeonMain.Rigidbody.AddForce(new Vector2(0, _inputFlyControl * FLIGHT_RESPONSIVENESS), ForceMode2D.Impulse);

        if (_timerBombCoolDown < _pigeonMain.Bomber.CoolDown) _timerBombCoolDown += Time.deltaTime;
    }
}