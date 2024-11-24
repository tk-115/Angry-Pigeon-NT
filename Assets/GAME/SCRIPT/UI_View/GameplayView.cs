using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(AudioSource))]
public class GameplayView : MonoBehaviour {
    public event Action OnButtonPauseClickEvent;

    public event Action OnButtonControllUPClickEvent;
    public event Action OnButtonControllDownClickEvent;
    public event Action OnButtonControllDeclickEvent;
    public event Action OnButtonControllBombClickEvent;

    public event Action OnButtonContinueClickEvent;
    public event Action OnButtonRestartClickEvent;
    public event Action OnButtonInMainMenuClickEvent;

    public event Action OnButtonResurrectionClickEvent;

    public event Action OnVoronaAlertComplete;

    [Inject] private LanguageControll _languageControll;

    [SerializeField] private TextMeshProUGUI _scores;
    [SerializeField] private TextMeshProUGUI _meters;
    [SerializeField] private TextMeshProUGUI _coins;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _currentBonusIcon;

    [SerializeField] private Image _voronaAlert;

    [SerializeField] private TextMeshProUGUI _resurrectionButtonText;

    [SerializeField] private TextMeshProUGUI _gameOverScoresTotal;
    [SerializeField] private TextMeshProUGUI _gameOverMetersTotal;
    [SerializeField] private TextMeshProUGUI _gameOverCoinsTotal;
    [SerializeField] private TextMeshProUGUI _gameOverNewRecordOrNot;

    [SerializeField] private AudioClip _voronaAlertClip;
    [SerializeField] private AudioClip _newBonusApplyClip;
    [SerializeField] private AudioClip _coinPickedUpClip;
    [SerializeField] private AudioClip _pauseEnterClip;

    private Color DEFAULT_SPRITE_COLOR = new Color(255, 255, 255, 255);
    private Color TRANSPARENT_SPRITE_COLOR = new Color(255, 255, 255, 0);
    private AudioSource _audioSource;
    private Animator _animator;

    public LanguageControll LanguageControll => _languageControll;

    public void Initialize() {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void UpdateLocalizationTable() => _languageControll.LoadLocaledTexts();

    public void ShowVoronaAlert(Vector3 position) {
        _audioSource.PlayOneShot(_voronaAlertClip);

        _voronaAlert.transform.position = position;
        _voronaAlert.color = DEFAULT_SPRITE_COLOR;

        _voronaAlert.transform.DOScale(
            new Vector3(1f, 1f, 1f), .5f).From(new Vector3(0.05f, 0.05f, 0.05f))
            .SetLoops(4, LoopType.Yoyo).OnComplete(() => {
                _voronaAlert.color = TRANSPARENT_SPRITE_COLOR;
                OnVoronaAlertComplete?.Invoke();
            });
    }

    public void SetRessurectionButtonText(string text) => _resurrectionButtonText.text = text;

    public void SetHealthBarValue(float value) => _healthBar.fillAmount = value;
    
    public void SetScores(int value) => _scores.text = value.ToString();

    public void SetMeters(int value) => _meters.text = value.ToString();

    public void SetCoins(int value) {
        if (value != 0) _audioSource.PlayOneShot(_coinPickedUpClip);
        _coins.text = value.ToString();
    }

    public void NewBonusSFXPlay() =>  _audioSource.PlayOneShot(_newBonusApplyClip);

    public void SetCurrentBonusIcon(Sprite sprite) {
        _currentBonusIcon.color = DEFAULT_SPRITE_COLOR;
        _currentBonusIcon.sprite = sprite;
    }

    public void ClearCurrentBonusIcon() => _currentBonusIcon.color = TRANSPARENT_SPRITE_COLOR;

    public void SetTotalScores(int value) => _gameOverScoresTotal.text = value.ToString();

    public void SetTotalMeters(int value) => _gameOverMetersTotal.text = value.ToString();

    public void SetTotalCoins(int value) => _gameOverCoinsTotal.text = value.ToString();

    public void SetRecordOrNot(bool flag) {
        if (flag == true) _gameOverNewRecordOrNot.text = _languageControll.GetLocaledTextByKey("go_new_record");
        else _gameOverNewRecordOrNot.text = _languageControll.GetLocaledTextByKey("go_no_new_record");
    }

    #region Animations
    //Common
    public void ShowGameplay() => _animator.Play("GameplayShow");

    public void HideGameplay() => _animator.Play("GameplayHide");

    public void OnHideGameplayComplete() => OnButtonInMainMenuClickEvent?.Invoke();

    //Gameover
    public void ShowGameOverPage() => _animator.Play("GameOverPageShow");

    public void HideGameOverPage() => _animator.Play("GameOverPageHide");

    //Pause
    public void ShowPausePage() => _animator.Play("PausePageShow");

    public void OnPauseShowComplete() {
        _audioSource.PlayOneShot(_pauseEnterClip);
        OnButtonPauseClickEvent?.Invoke();
    }

    public void HidePausePage() => _animator.Play("PausePageHide");
    #endregion

    #region Buttons Events

    //Pause
    public void OnButtonPauseClicked() => ShowPausePage();

    //Controlls
    public void OnButtonControllUPClick() => OnButtonControllUPClickEvent?.Invoke();

    public void OnButtonControllDownClick() => OnButtonControllDownClickEvent?.Invoke();

    public void OnButtonControllBombClick() => OnButtonControllBombClickEvent?.Invoke();

    public void OnButtonControllDeclick() => OnButtonControllDeclickEvent?.Invoke();

    //Pause page
    public void OnButtonContinueClick() => OnButtonContinueClickEvent?.Invoke();

    public void OnButtonRestartClick() => OnButtonRestartClickEvent?.Invoke();

    public void OnButtonInMainMenuClick() {
        Time.timeScale = 1f;
        HideGameplay();
    }

    //Game over page
    public void OnButtonResurrectionClick() => OnButtonResurrectionClickEvent?.Invoke();    

    #endregion
}