using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class AdsManagerSystem : BaseMonoSystem
{
    public static AdsManagerSystem Instance;

    [SerializeField] private string _adUnitId = "R-M-DEMO-interstitial";
    
    private String message = "";

    private Interstitial interstitial;
    
    public override void Init(AppData data)
    {
        base.Init(data);
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        
#if !UNITY_EDITOR
        RequestInterstitial();
#endif
    }

    public void TryShowVideoAds()
    {
#if !UNITY_EDITOR
        if (BuildConfig.Yandex_Ads)
        {
            ShowInterstitial();
        }
#endif
    }
    
    private void RequestInterstitial()
    {
        //Sets COPPA restriction for user age under 13
        MobileAds.SetAgeRestrictedUser(true);

        // Replace demo R-M-DEMO-interstitial with actual Ad Unit ID
        string adUnitId = _adUnitId;

        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        this.interstitial = new Interstitial(adUnitId);

        this.interstitial.OnInterstitialLoaded += this.HandleInterstitialLoaded;
        this.interstitial.OnInterstitialFailedToLoad += this.HandleInterstitialFailedToLoad;
        this.interstitial.OnReturnedToApplication += this.HandleReturnedToApplication;
        this.interstitial.OnLeftApplication += this.HandleLeftApplication;
        this.interstitial.OnAdClicked += this.HandleAdClicked;
        this.interstitial.OnInterstitialShown += this.HandleInterstitialShown;
        this.interstitial.OnInterstitialDismissed += this.HandleInterstitialDismissed;
        this.interstitial.OnImpression += this.HandleImpression;
        this.interstitial.OnInterstitialFailedToShow += this.HandleInterstitialFailedToShow;

        this.interstitial.LoadAd(this.CreateAdRequest());
        this.DisplayMessage("Interstitial is requested");
    }

    private void ShowInterstitial()
    {
        this.interstitial.Show();
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    private void DisplayMessage(String message)
    {
        this.message = message + (this.message.Length == 0 ? "" : "\n--------\n" + this.message);
        MonoBehaviour.print(message);
    }

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleInterstitialLoaded event received");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailureEventArgs args)
    {
        this.DisplayMessage("HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleReturnedToApplication(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleReturnedToApplication event received");
    }

    public void HandleLeftApplication(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleLeftApplication event received");
    }

    public void HandleAdClicked(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleAdClicked event received");
    }

    public void HandleInterstitialShown(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleInterstitialShown event received");
    }

    public void HandleInterstitialDismissed(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleInterstitialDismissed event received");
    }

    public void HandleImpression(object sender, ImpressionData impressionData)
    {
        var data = impressionData == null ? "null" : impressionData.rawData;
        this.DisplayMessage("HandleImpression event received with data: " + data);
    }

    public void HandleInterstitialFailedToShow(object sender, AdFailureEventArgs args)
    {
        this.DisplayMessage("HandleInterstitialFailedToShow event received with message: " + args.Message);
    }

    #endregion
}
