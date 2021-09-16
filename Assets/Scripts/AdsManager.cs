using System;
using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using EasyMobile;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    public ScriptLinkerSO linker;

    public void Init()
    {

        instance = this;


        StartCoroutine(CR_Init());
    }

    IEnumerator CR_Init()
    {
        print("WAITING");
        linker.ads_ready = false;
        while (!Advertising.IsInitialized())
        {
            Advertising.Initialize();
            yield return new WaitForSeconds(1);
        }
        print("DONE WAITING");

        while (!Advertising.AdMobClient.IsInitialized)
        {
            Advertising.AdMobClient.Init();
            yield return new WaitForSeconds(1);
        }

        linker.ads_ready = true;
        linker.AdsReady.Invoke();
        SetActiveBanner(true);
    }


    public void SetActiveBanner(bool show)
    {

        if (!InAppPurchasing.IsProductOwned(EM_IAPConstants.Product_Remove_Ads))
        {
            if (show )
            {
                Advertising.AdMobClient.ShowBannerAd(BannerAdPosition.Bottom,BannerAdSize.IABBanner);
            }
            else
            {
                Advertising.AdMobClient.HideBannerAd();
            }
        }
        else
        {
            Advertising.AdMobClient.HideBannerAd();
            print("destroy ads");
        }

    }


    [Button]
    public void ShowInterstitial()
    {
        bool isReady = Advertising.IsInterstitialAdReady();
        if (isReady && !InAppPurchasing.IsProductOwned(EM_IAPConstants.Product_Remove_Ads))
        {
            Advertising.ShowInterstitialAd();
        }
    }
    Action reward_call;
    Action reward_call_fail;

    int i = 0;

    public void ShowSkipAdd()
    {
        if (i%3 == 0)
        {
            ShowRewarded(null);
        }
        else
        {
            ShowInterstitial();
        }
    }

    public void ShowRewarded(Action _reward_call, Action _reward_call_fail = null)
    {
        bool isReady = Advertising.IsRewardedAdReady();
        if (isReady)
        {
            reward_call = _reward_call;
            reward_call_fail = _reward_call_fail;
            Advertising.ShowRewardedAd();
        }
    }

    void InterstitialAdCompletedHandler(InterstitialAdNetwork network, AdPlacement location)
    {

    }


    void RewardedAdCompletedHandler(RewardedAdNetwork network, AdPlacement location)
    {
        if (reward_call != null)
        {
            reward_call.Invoke();
        }
    }

    void RewardedAdSkippedHandler(RewardedAdNetwork network, AdPlacement location)
    {
        if (reward_call_fail != null)
        {
            reward_call_fail.Invoke();
        }
    }


    void OnEnable()
    {
        Advertising.InterstitialAdCompleted += InterstitialAdCompletedHandler;
        Advertising.RewardedAdCompleted += RewardedAdCompletedHandler;
        Advertising.RewardedAdSkipped += RewardedAdSkippedHandler;
    }

    void OnDisable()
    {
        Advertising.InterstitialAdCompleted -= InterstitialAdCompletedHandler;
        Advertising.RewardedAdCompleted -= RewardedAdCompletedHandler;
        Advertising.RewardedAdSkipped -= RewardedAdSkippedHandler;
    }

}
