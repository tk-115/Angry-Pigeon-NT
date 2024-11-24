using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdsManager : MonoBehaviour {

    public event Action OnRewarderAdComplete;
    public event Action OnRewarderAdFailed;

    public string appId = "ca-app-pub-1573041903763000~4903028889";     // test: "ca-app-pub-3940256099942544~3347511713";
    string interId = "ca-app-pub-1573041903763000/8650702200";          // test: "ca-app-pub-3940256099942544/1033173712";
    string rewardedId = "ca-app-pub-1573041903763000/4689779033";       // test: "ca-app-pub-3940256099942544/5224354917";

    private InterstitialAd _interAd;
    private RewardedAd _rewardedAd;

    private void Start() {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => { 
            Debug.Log("Ads init ok");
        });
    }

    #region Interstitial

    public void LoadInterstitialAd() {
        if (_interAd != null) {
            _interAd.Destroy();
            _interAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(interId, adRequest, (InterstitialAd ad, LoadAdError error) => {
            if (error != null || ad == null) {
                Debug.Log("inter ad error " + error);
                
                return;
            }
            _interAd = ad;
            InterstitialEvent(_interAd);

            //show
            if (_interAd != null && _interAd.CanShowAd()) {
                _interAd.Show();
                Debug.Log("inter ok");
            } else {
                Debug.Log("inter ad not ready( ");
            }
        });

    }

    private void InterstitialEvent(InterstitialAd interstitialAd) {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #endregion

    #region Rewarded

    public void LoadRewardedAd() {
        if (_rewardedAd != null) {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(rewardedId, adRequest, (RewardedAd ad, LoadAdError error) => {
            if (error != null || ad == null) {
                Debug.Log("rewarded ad failed to load " + error);
                OnRewarderAdFailed?.Invoke();
                return;
            }
            Debug.Log("rewarded ad load ok");
            _rewardedAd = ad;
            RewardedAdEvents(_rewardedAd);

            //show
            if (_rewardedAd != null && _rewardedAd.CanShowAd()) {
            
                _rewardedAd.Show((Reward reward) => {
                    //ништяк юзеру
                    Debug.Log("player has ressurection!");
                    OnRewarderAdComplete?.Invoke();
                });
            } else {
                OnRewarderAdFailed?.Invoke();
                Debug.Log("reward ad not ready");
            }
        });
    }

    private void RewardedAdEvents(RewardedAd ad) {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) => {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () => {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () => {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () => {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () => {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) => {
            OnRewarderAdFailed?.Invoke();
            Debug.LogError("Rewarded ad failed to open full screen content " + "with error : " + error);
        };
    }
    #endregion
}
