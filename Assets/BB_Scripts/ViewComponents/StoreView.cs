using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AdsControl;
using Gley.MobileAds;
using System;

public class StoreView : BaseView
{
    //gift item
    public RectTransform giftPackageViewRoot;

    //coin item

    public RectTransform coinPackageViewRoot;

    //life item
    public RectTransform lifePackageViewRoot;

    public StoreLayer storelayer;

    public Text coinTxt;

    public Text starTxt;

    public Text heartTxt;

    public Text heartFullTxt;

    public Text freeAdsValueTxt;

    public Text freeAdsTimerTxt;

    public override void InitView()
    {
        UpdateCoinTxt();
        UpdateStarTxt();
    }

    public override void Start()
    {
        LoadItem();
    }

    public override void Update()
    {
        
    }

    private void LoadItem()
    {
        for (int i = 0; i < storelayer.giftPackageList.Count; i++)
        {
            GiftPackageView itemView = Instantiate(storelayer.giftPackageViewPrefab);
            itemView.transform.parent = giftPackageViewRoot;
            itemView.transform.localScale = Vector3.one;
            itemView.ShowView(storelayer.giftPackageList[i]);
        }

        for (int i = 0; i < storelayer.coinPackageList.Count; i++)
        {
            CoinPackageView itemView = Instantiate(storelayer.coinPackageViewPrefab);
            itemView.transform.parent = coinPackageViewRoot;
            itemView.transform.localScale = Vector3.one;
            itemView.ShowView(storelayer.coinPackageList[i]);
        }

        for (int i = 0; i < storelayer.lifePackageList.Count; i++)
        {
            LifePackageView itemView = Instantiate(storelayer.lifePackageViewPrefab);
            itemView.transform.parent = lifePackageViewRoot;
            itemView.transform.localScale = Vector3.one;
            itemView.ShowView(storelayer.lifePackageList[i]);
        }
    }

    public void ClosePopup()
    {
        AudioManager.instance.btnSound.Play();
        HideView();
    }

    public void UpdateCoinTxt()
    {
        coinTxt.text = GameManager.Instance.currentCoin.ToString();
    }

    public void UpdateStarTxt()
    {
        starTxt.text = GameManager.Instance.currentStar.ToString();
    }

    public void OnLivesChanged()
    {
        heartTxt.text = GameManager.Instance.livesManager.LivesText;
    }

    public void OnTimeToNextLifeChanged()
    {
        heartFullTxt.text = GameManager.Instance.livesManager.RemainingTimeString;
    }

    public void UpdateFreeAdsTxt()
    {
        freeAdsValueTxt.text = GameManager.Instance.freeAdsTimer.LivesText;
    }

    public void UpdateFreeAdsTimerTxt()
    {
        freeAdsTimerTxt.text = GameManager.Instance.freeAdsTimer.RemainingTimeString;
    }

    public void FreeCoin()
    {
        AudioManager.instance.btnSound.Play();
        if (GameManager.Instance.freeAdsTimer.freeAdsNumber > 0)
        {
           


            API.ShowRewardedVideo(completeMethod);
        }

    }

    private void completeMethod(bool s)
    {
        if (s)
        {
            GameManager.Instance.uiManager.storeLayer.EarnFreeCoin();
        }
    }

    public void Restore()
    {
        AudioManager.instance.btnSound.Play();
        IAPManager.instance.RestorePurchases();
        Debug.Log("Restore");
    }

    public void RemoveAds()
    {
        AudioManager.instance.btnSound.Play();
        GameManager.Instance.uiManager.storeLayer.RemoveAds();
    }
}
