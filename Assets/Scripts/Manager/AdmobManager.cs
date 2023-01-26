using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobManager : Singleton<AdmobManager>
{
    public bool isTestMode;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        var requestConfiguration = new RequestConfiguration
           .Builder()
           .SetTestDeviceIds(new List<string>() { "1DF7B7CC05014E8" }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        LoadFrontAd();
    }

    void Update()
    {
    }

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";//테스트용 애드몹 아이디
    const string frontID = "ca-app-pub-5708876822263347/4578528355";//출시 후 사용 애드몹 아이디
    InterstitialAd frontAd;

    public void LoadFrontAd()
    {
        frontAd = new InterstitialAd(isTestMode ? frontTestID : frontID);
        frontAd.LoadAd(GetAdRequest());
        frontAd.OnAdClosed += (sender, e) =>
        {
            IngameUIManager.Instance.DoneAD();
        };
    }

    public void ShowFrontAd()
    {
        frontAd.Show();
        LoadFrontAd();
    }
}
