using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EasyMobile;
using System;

public class AdButton : MonoBehaviour
{
    public Image indicator;
    public TextMeshProUGUI status_txt;
    public Button button;
    public ScriptLinkerSO linker;

    public bool loaded = false;

    private void Start()
    {
        button = GetComponent<Button>();

        loaded = linker.ads_ready;

        linker.AdsReady += () =>
        {

            loaded = true;
            UpdateUI();
        };

        Advertising.AdMobClient.OnRewardedAdLoaded += OnLoaded;
        Advertising.AdMobClient.OnRewardedAdClosed += OnShown;
        Advertising.AdMobClient.OnRewardedAdFailedToLoad += OnShown;

        UpdateUI();
    }



    private void OnShown(object sender, EventArgs e)
    {
        indicator.color = Color.red;
        status_txt.SetText("NOT READY");
        button.interactable = false;


    }

    private void OnLoaded(object sender, EventArgs e)
    {
        indicator.color = Color.black;
        status_txt.SetText("READY");
        button.interactable = true;
    }

    private void UpdateUI()
    {
        if (loaded)
        {
            if (Advertising.IsRewardedAdReady())
            {
                OnLoaded(null, null);
            }
            else
            {
                OnShown(null, null);
            }
        }
    }

    private void OnEnable()
    {
        UpdateUI();
    }
}
