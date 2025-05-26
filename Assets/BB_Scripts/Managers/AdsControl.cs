using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Gley.MobileAds;

public class AdsControl : MonoBehaviour
{


    private void Start()
    {
        API.Initialize(OnInitialized);

    }

    private void OnInitialized()
    {
       // API.ShowBanner(BannerPosition.Bottom, BannerType.Adaptive);

        if (!API.GDPRConsentWasSet())
        {
            API.ShowBuiltInConsentPopup(PopupCloseds);
        }
    }

    private void PopupCloseds()
    {

    }


    private void OnApplicationPause(bool pause)
    {
        if (pause == false)
        {
            Gley.MobileAds.API.ShowAppOpen();
        }
    }
   
}

