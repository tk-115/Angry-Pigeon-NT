using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StorePageView : MonoBehaviour {
    public event Action OnButtonBackFromStoreClickEvent;
    public event Action<int> OnButtonSkinClickEvent;

    public event Action OnStoreShowEvent;

    [SerializeField] private TextMeshProUGUI _storeScoresRecord, _storeMetersRecord, _storeCoins;
    [SerializeField] private List<StoreSkinUnit> _skinUnits;
    [SerializeField] private Sprite _selected, _locked, _unlocked;

    public List<StoreSkinUnit> SkinUnits => _skinUnits;
    public Sprite SkinSelected => _selected;
    public Sprite SkinLocked => _locked;
    public Sprite SkinUnlocked => _unlocked;

    private Animator _animator;

    public void Initialize() => _animator = GetComponent<Animator>();

    public void ShowStore() => _animator.Play("StoreShow");

    public void OnStoreShow() => OnStoreShowEvent?.Invoke();

    public void HideStore() => _animator.Play("StoreHide");

    public void SetStoreScoresRecord(int scores) => _storeScoresRecord.text = scores.ToString();

    public void SetStoreMetersRecord(int scores) => _storeMetersRecord.text = scores.ToString();

    public void SetStoreCoinsRecord(int scores) => _storeCoins.text = scores.ToString();

    public void OnButtonBackFromStoreClicked() => OnButtonBackFromStoreClickEvent?.Invoke();

    public void OnButtonSkinClicked(int skinid) => OnButtonSkinClickEvent?.Invoke(skinid);
}