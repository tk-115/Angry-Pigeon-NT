using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;

public class MainMenuState : IState {
    private const float SOUND_DISABLED = 0.0001f;
    private const float SOUND_ENABLED = 1f;
    private const int EASTEREGG_CLICK_COUNT = 7;

    private IStateSwicher _switcher;
    private MainPageView _mainMenuView;
    private PlayerData _playerData;
    private AudioMixer _mixer;

    private bool _sfxEnabled = false;
    private bool _musicEnabled = false;
    private int _easterEggClick = 0;

    public MainMenuState(IStateSwicher switcher, MainPageView mainMenuView, 
        PlayerData playerData, AudioMixer mixer) {
        _switcher = switcher;
        _mainMenuView = mainMenuView;
        _playerData = playerData;
        _mixer = mixer;
    }

    public void Enter() {
        //количество кликов по логотипу TK936 обнулить
        _easterEggClick = 0;

        //Загрузить самые новые данные о рекордах игрока
        _playerData.LoadData();

        //Выбрать сохраненный текущий язык и отобразить
        SetCurrentLanguage(_playerData.Settings_languageID);
        _mainMenuView.SetLanguageSelectedID(_playerData.Settings_languageID);

        //Включить или выключить звук
        if (_playerData.Settings_SFX == SOUND_ENABLED) {
            _sfxEnabled = true;
            _mixer.SetFloat("SFXVolume", Mathf.Log10(SOUND_ENABLED) * 20);
        } else { 
            _sfxEnabled = false;
            _mixer.SetFloat("SFXVolume", Mathf.Log10(SOUND_DISABLED) * 20);
        }
        if (_playerData.Settings_Music == SOUND_ENABLED) {
            _musicEnabled = true;
            _mixer.SetFloat("MusicVolume", Mathf.Log10(SOUND_ENABLED) * 20);
        } else {
            _musicEnabled = false;
            _mixer.SetFloat("MusicVolume", Mathf.Log10(SOUND_DISABLED) * 20);
        }

        //Применить значения в view главного меню
        _mainMenuView.SetSFXTumbler(_sfxEnabled);
        _mainMenuView.SetMusicTumbler(_musicEnabled);
        _mainMenuView.SetScoresRecord(_playerData.Record_Scores);
        _mainMenuView.SetMetersRecord(_playerData.Record_Meters);
        _mainMenuView.SetConinsRecord(_playerData.Coins);

        //подписаться на кнопки главного меню
        _mainMenuView.OnButtonPlayClickEvent += OnButtonPlayClicked;
        _mainMenuView.OnButtonStoreClickEvent += OnButtonStoreClicked;
        _mainMenuView.OnButtonExitClickEvent += OnButtonExitClicked;
        _mainMenuView.OnButtonSFXClickEvent += OnButtonSettingsSFXClicked;
        _mainMenuView.OnButtonMusicClickEvent += OnButtonSettingsMusicClicked;
        _mainMenuView.OnButtonLanguageClickEvent += OnButtonLanguageChangeClicked;
        _mainMenuView.OnEasterEggButtonClickEvent += OnButtonEasterEggClicked;
        _mainMenuView.ShowMainMenu();
    }

    public void Exit() {
        _mainMenuView.OnButtonPlayClickEvent -= OnButtonPlayClicked;
        _mainMenuView.OnButtonStoreClickEvent -= OnButtonStoreClicked;
        _mainMenuView.OnButtonExitClickEvent -= OnButtonExitClicked;
        _mainMenuView.OnButtonSFXClickEvent -= OnButtonSettingsSFXClicked;
        _mainMenuView.OnButtonMusicClickEvent -= OnButtonSettingsMusicClicked;
        _mainMenuView.OnButtonLanguageClickEvent -= OnButtonLanguageChangeClicked;
        _mainMenuView.OnEasterEggButtonClickEvent -= OnButtonEasterEggClicked;

        _mainMenuView.HideMainMenu();
    }

    //Логика кнопок
    public void OnButtonPlayClicked() => _switcher.SwitchState<GameplayState>();

    public void OnButtonStoreClicked() => _switcher.SwitchState<StoreState>();

    public void OnButtonExitClicked() => Application.Quit();

    public void OnButtonSettingsSFXClicked() {
        _sfxEnabled = !_sfxEnabled;

        if (_sfxEnabled == true) {
            _mixer.SetFloat("SFXVolume", Mathf.Log10(SOUND_ENABLED) * 20);
            _playerData.Settings_SFX = SOUND_ENABLED;
        } else {
            _mixer.SetFloat("SFXVolume", Mathf.Log10(SOUND_DISABLED) * 20);
            _playerData.Settings_SFX = SOUND_DISABLED;
        }
        _playerData.SaveData();
        _mainMenuView.SetSFXTumbler(_sfxEnabled);
    }

    public void OnButtonSettingsMusicClicked() {
        _musicEnabled = !_musicEnabled;

        if (_musicEnabled == true) {
            _mixer.SetFloat("MusicVolume", Mathf.Log10(SOUND_ENABLED) * 20);
            _playerData.Settings_Music = SOUND_ENABLED;
        } else {
            _mixer.SetFloat("MusicVolume", Mathf.Log10(SOUND_DISABLED) * 20);
            _playerData.Settings_Music = SOUND_DISABLED;
        }
        _playerData.SaveData();
        _mainMenuView.SetMusicTumbler(_musicEnabled);
    }

    public void OnButtonLanguageChangeClicked(int languageID) {
        SetCurrentLanguage(languageID);
        _playerData.Settings_languageID = languageID;
        _playerData.SaveData();
        _mainMenuView.SetLanguageSelectedID(languageID);
    }

    private void SetCurrentLanguage(int languageID) {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        var locale = locales[languageID];
        LocalizationSettings.SelectedLocale = locale;
    }

    private void OnButtonEasterEggClicked() {
        _easterEggClick++;
        if (_easterEggClick >= EASTEREGG_CLICK_COUNT) {
            _playerData.ModData();
            _switcher.SwitchState<MainMenuState>();
        }
    }

    void IState.Update() { }
}