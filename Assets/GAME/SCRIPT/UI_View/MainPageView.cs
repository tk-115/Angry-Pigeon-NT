using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))] 
public class MainPageView : MonoBehaviour {
    public event Action OnButtonPlayClickEvent;
    public event Action OnButtonStoreClickEvent;
    public event Action OnButtonExitClickEvent;
    public event Action OnButtonSFXClickEvent;
    public event Action OnButtonMusicClickEvent;
    public event Action<int> OnButtonLanguageClickEvent;

    public event Action OnEasterEggButtonClickEvent;

    [SerializeField] private TextMeshProUGUI _scoresRecord, _metersRecord, _coins;
    [SerializeField] private Image _sFXImage, _musicImage, _languageRUImage, _languageENImage;
    [SerializeField] private Sprite _languageRU, _languageRUSelected, _languageEN, _languageENSelected;
    [SerializeField] private Sprite _swicherOn, _swicherOff;

    private Animator _animator;

    public void Initialize() => _animator = GetComponent<Animator>();

    public void ShowMainMenu() => _animator.Play("MainMenuShow");
    
    public void HideMainMenu() => _animator.Play("MainMenuHide");

    public void SetScoresRecord(int scores) => _scoresRecord.text = scores.ToString();

    public void SetMetersRecord(int meters) => _metersRecord.text = meters.ToString();

    public void SetConinsRecord(int coins) => _coins.text = coins.ToString();

    public void SetSFXTumbler(bool flag) {
        if (flag == true) _sFXImage.sprite = _swicherOn; else _sFXImage.sprite = _swicherOff;
    }

    public void SetMusicTumbler(bool flag) {
        if (flag == true) _musicImage.sprite = _swicherOn; else _musicImage.sprite = _swicherOff;
    }

    public void SetLanguageSelectedID(int languageID) { 
        switch (languageID) {
            case 0:
                _languageRUImage.sprite = _languageRUSelected;
                _languageENImage.sprite = _languageEN;
                break;
            case 1:
                _languageRUImage.sprite = _languageRU;
                _languageENImage.sprite = _languageENSelected;
                break;
            default:
                _languageRUImage.sprite = _languageRUSelected;
                _languageENImage.sprite = _languageEN;
                languageID = 0;
                break;
        }
    }

    public void OnButtonPlayClicked() => OnButtonPlayClickEvent?.Invoke();

    public void OnButtonStoreClicked() => OnButtonStoreClickEvent?.Invoke();

    public void OnButtonExitClicked() => OnButtonExitClickEvent?.Invoke();

    public void OnButtonSFXClicked() => OnButtonSFXClickEvent?.Invoke();

    public void OnButtonMusicClicked() => OnButtonMusicClickEvent?.Invoke();

    public void OnButtonLanguageChangeClicked(int languageID) => OnButtonLanguageClickEvent?.Invoke(languageID);

    public void OnButtonEasterEggClicked() => OnEasterEggButtonClickEvent?.Invoke();
}