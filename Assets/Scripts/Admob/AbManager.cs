using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AbManager : MonoBehaviour
{
    private BannerView bannerAd;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    // THIS IS A TEST ADS ID, WHEN YOU WANT TO PUBLISH YOUR GAME JUST REPLACE THESE ID'S WITH YOUR OWN ADMOB ID'S

    #region caa IOS

    private string IOSBannerAdId = "ca-app-pub-9151001313181438/1536386561";
    private string IOSIntersitialAdId = "ca-app-pub-9151001313181438/2278622597";
    private string IOSRewardedVideoAdId = "ca-app-pub-9151001313181438/1344814877";

    #endregion

    #region caa Android

    private string AndroidBannerAdId = "ca-app-pub-9151001313181438/4354121591";
    private string AndroidIntersitialAdId = "ca-app-pub-9151001313181438/6213519248";
    private string AndroidRewardedVideoAdId = "ca-app-pub-9151001313181438/1072873200";

    #endregion

    // Start is called before the first frame update
    public bool RunAdmob = false;
    public Canvas CanvasGame;
    public GameObject Audio;

    void Start()
    {
        MobileAds.Initialize(InitializationStatus => { });
        this.RequestBanner();
    }

    public bool getRunAdmob()
    {
        return this.RunAdmob;
    }

    private void HandleOnAdClosed(object sender, EventArgs args)
    {
        CanvasGame.GetComponent<bussGame>().AdmobGameIntersitial();
        CanvasGame.gameObject.SetActive(true);
        Audio.GetComponent<bussSound>().SoundMenu(false);
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
        this.CanvasGame.GetComponent<bussGame>().RewardCallback();
        CanvasGame.gameObject.SetActive(true);
        Audio.GetComponent<bussSound>().SoundMenu(false);
    }

    private void RewardedAd_OnAdLoaded(object sender, EventArgs e)
    {
        this.Audio.GetComponent<bussSound>().SoundMenu(true);
        CanvasGame.gameObject.SetActive(false);
        rewardedAd.Show();
    }

    // used in start func
    // when you want to show your banner on screen
    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = AndroidBannerAdId ;
#elif UNITY_IPHONE
        string adUnitId = IOSBannerAdId;
#else
        string adUnitId = "unexpected_platform";
#endif
        this.bannerAd = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
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
#if UNITY_ANDROID
        string adUnitId = AndroidIntersitialAdId ;
#elif UNITY_IPHONE
        string adUnitId = IOSIntersitialAdId;
#else
        string adUnitId = "unexpected_platform";
#endif

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
            CanvasGame.gameObject.SetActive(false);
            Audio.GetComponent<bussSound>().SoundMenu(true);
            interstitial.Show();
        }
        else
        {
            //no load interstiaAd
            CanvasGame.gameObject.SetActive(true);
            Audio.GetComponent<bussSound>().SoundMenu(false);
        }

        this.interstitial.OnAdClosed += HandleOnAdClosed;
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        bannerAd.Destroy();
    }

    // load rewarded video ads
    public void LoadRewardedVideoAds()
    {
#if UNITY_ANDROID
        string adUnitId = AndroidRewardedVideoAdId;
#elif UNITY_IPHONE
        string adUnitId = IOSRewardedVideoAdId;
#else
        string adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdLoaded += RewardedAd_OnAdLoaded;
        ;
        this.rewardedAd.OnUserEarnedReward += RewardedAd_OnUserEarnedReward;
        ;
        this.rewardedAd.OnAdClosed += RewardedAd_OnAdClosed;
        ;
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(this.CreateAdRequest());
        bannerAd.Destroy();
    }
}