public class StoreState : IState {

    private IStateSwicher _switcher;
    private PlayerData _playerData;
    private StorePageView _storePageView;
    private AdsManager _adsManager;

    public StoreState(IStateSwicher switcher, PlayerData playerData, StorePageView storePageView, AdsManager adsManager) {
        _switcher = switcher;
        _playerData = playerData;
        _storePageView = storePageView;
        _adsManager = adsManager;
    }

    public void Enter() {
        //подписаться на кнопки
        _storePageView.OnButtonBackFromStoreClickEvent += OnButtonBackClicked;
        _storePageView.OnButtonSkinClickEvent += OnButtonSkinClicked;

        //заполнить рекорды игрока
        _storePageView.SetStoreScoresRecord(_playerData.Record_Scores);
        _storePageView.SetStoreMetersRecord(_playerData.Record_Meters);
        _storePageView.SetStoreCoinsRecord(_playerData.Coins);

        //открыть клетки в соответствии с рекордами и применить примененный скин
        //заполнить необходимые значения рекордов для каждого скина
        for (int i = 0; i < _storePageView.SkinUnits.Count; i++) {
            _storePageView.SkinUnits[i].NeedScoreText.text = _storePageView.SkinUnits[i].Score.ToString();
            _storePageView.SkinUnits[i].NeedMetersText.text = _storePageView.SkinUnits[i].Meters.ToString();
            _storePageView.SkinUnits[i].NeedCoinsText.text = _storePageView.SkinUnits[i].Coins.ToString();

            //открыть, если требуемый рекорд был достигнут
            if (CheckAvailabilityPigeonByID(i)) {
                //убираем клетку
                _storePageView.SkinUnits[i].Kletka_back.enabled = false;
                _storePageView.SkinUnits[i].Kletka_front.enabled = false;

                //Так как скин открыт, он может быть применен
                if (_playerData.SelectedPigeonID == i) _storePageView.SkinUnits[i].Status.sprite = _storePageView.SkinSelected;
                //Или открыт для применения
                else _storePageView.SkinUnits[i].Status.sprite = _storePageView.SkinUnlocked;
            }
            //Если рекорд не был достигнут, повесить замок
            else {
                _storePageView.SkinUnits[i].Status.sprite = _storePageView.SkinLocked;
            }
        }
        _storePageView.OnStoreShowEvent += OnStoreShow;
        _storePageView.ShowStore();
    }

    //Event окончания показа магазина
    public void OnStoreShow() {
        //показать рекламу, если не получится то и ***, юзер в оффлайне все равно может пользоваться магазином
        _adsManager.LoadInterstitialAd();
    }

    public void Exit() {
        _storePageView.OnButtonBackFromStoreClickEvent -= OnButtonBackClicked;
        _storePageView.OnButtonSkinClickEvent -= OnButtonSkinClicked;

        _storePageView.OnStoreShowEvent -= OnStoreShow;

        _storePageView.HideStore();
    }

    public void OnButtonBackClicked() => _switcher.SwitchState<MainMenuState>();

    public void OnButtonSkinClicked(int skinid) {
        if (skinid == _playerData.SelectedPigeonID) return;

        //Проверяем, можно ли применить данный скин, если да, применяем
        if (CheckAvailabilityPigeonByID(skinid)) {
            //view
            _storePageView.SkinUnits[_playerData.SelectedPigeonID].Status.sprite = _storePageView.SkinUnlocked;
            _storePageView.SkinUnits[skinid].Status.sprite = _storePageView.SkinSelected;

            //применить скин и сохранить PlayerData
            _playerData.SelectedPigeonID = skinid;
            _playerData.SaveData();
        }
        //если нет ниче не делаем
    }
    
    void IState.Update() { }

    private bool CheckAvailabilityPigeonByID(int id) {
        if (_storePageView.SkinUnits[id].Score <= _playerData.Record_Scores &&
            _storePageView.SkinUnits[id].Meters <= _playerData.Record_Meters &&
            _storePageView.SkinUnits[id].Coins <= _playerData.Coins)
        {
            return true;
        }
        return false;
    }
}