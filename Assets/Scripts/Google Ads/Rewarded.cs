using UnityEngine;
using GoogleMobileAds.Api;
using Manager;

public class Rewarded : MonoBehaviour
{
    private RewardedAd _rewardedAd;

#if UNITY_ANDROID
    private string _adUnityId = "ca-app-pub-7876825000881029/7983606888";
#else
    private string _adUnityId = "unused";
#endif

    private void Awake()
    {
        MobileAds.Initialize((status => { }));
        CreateRewardedAd();

        // Subscribe events
        GameManager.Instance.playerWinEventChannel.OnEventRaised += ShowRewardedAd;
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null");
            return;
        }

        if (GameManager.Instance.playerWinEventChannel == null)
        {
            Debug.LogError("playerWinEventChannel is null");
            return;
        }

        GameManager.Instance.playerWinEventChannel.OnEventRaised -= ShowRewardedAd;
    }
    private void CreateRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        AdRequest adRequest = new AdRequest();

        RewardedAd.Load(_adUnityId, adRequest, (RewardedAd ad, LoadAdError error) => OnAdLoad(ad, error));
        Debug.Log("Created Rewarded Ad");
    }

    private void ShowRewardedAd()
    {
        const string rewardMsg = "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((reward =>
            {
                // TODO: Reward the user.
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
            }));
        }
    }

    #region Handles Ad Events

    private void OnAdLoad(RewardedAd ad, LoadAdError error)
    {
        if (error != null || ad == null)
        {
            Debug.LogError("Rewarded ad failed to load with error: " + error);
            return;
        }

        Debug.Log("Rewarded ad loaded with response: " + ad.GetResponseInfo());
        _rewardedAd = ad;

        SoundManager.Instance.StopBackgroundMusic();
    }

    #endregion
}