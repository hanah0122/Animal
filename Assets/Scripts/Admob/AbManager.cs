using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections;
public class AbManager : MonoBehaviour
{
    private BannerView bannerAd;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    // THIS IS A TEST ADS ID, WHEN YOU WANT TO PUBLISH YOUR GAME JUST REPLACE THESE ID'S WITH YOUR OWN ADMOB ID'S
    private string BannerAdId = "ca-app-pub-3940256099942544/6300978111";
    private string IntersitialAdId = "ca-app-pub-3940256099942544/1033173712";
    private string RewardedVideoAdId = "ca-app-pub-3940256099942544/5224354917";

    // Start is called before the first frame update


    void Start()
    {
        MobileAds.Initialize(InitializationStatus => { });
        this.RequestBanner();
    }
    private void HandleOnAdClosed(object sender, EventArgs args)
    {
        this.RequestBanner();
    }

    private void HandleOnAdLoaded(object sender, EventArgs args)
    {
        interstitial.Show();
    }
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    private void RewardedAd_OnAdClosed(object sender, EventArgs e)
    {
        this.RequestBanner();
    }

    private void RewardedAd_OnUserEarnedReward(object sender, Reward e)
    {
        Debug.Log("RewardedAd_OnUserEarnedReward");
        // reward your user
    }

    private void RewardedAd_OnAdLoaded(object sender, EventArgs e)
    {
        rewardedAd.Show();

    }

    // used in start func
    // when you want to show your banner on screen
    private void RequestBanner()
    {
        string adUnityId = BannerAdId;
        this.bannerAd = new BannerView(adUnityId, AdSize.SmartBanner, AdPosition.Bottom);
        this.bannerAd.LoadAd(this.CreateAdRequest());

    }

    public void CloseBanner()
    {
        bannerAd.Destroy();
    }

    // Request Intersitial ads

    // first Request to load Intersitial then show Intersitial ads
    public void ShowIntersitialAds()
    {
        Debug.Log("okkkkkk");
        string adUnitId = IntersitialAdId;

        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Load an Intersitial ad
        this.interstitial.LoadAd(this.CreateAdRequest());

        if (this.interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            Debug.Log("Not ready yet");
        }
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        bannerAd.Destroy();

    }

    // load rewarded video ads
    public void LoadRewardedVideoAds()
    {
#if UNITY_ANDROID
    	string adUnitId = RewardedVideoAdId;
#elif UNITY_IPHONE
        	string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdLoaded += RewardedAd_OnAdLoaded; ;
        this.rewardedAd.OnUserEarnedReward += RewardedAd_OnUserEarnedReward; ;
        this.rewardedAd.OnAdClosed += RewardedAd_OnAdClosed; ;


        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(this.CreateAdRequest());
        bannerAd.Destroy();
    }

}
