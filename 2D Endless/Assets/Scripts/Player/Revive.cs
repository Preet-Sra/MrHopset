using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour
{
    AudioSource _audio;
    private void OnEnable()
    {
        _audio = GetComponent<AudioSource>();
        _audio.volume = SoundManager.instance.GetSFXVolume();
    }

    public void RewardButtonClicked()
    {
        SoundManager.instance.PlayUiButtonSound();
        AdmobAdsScript.instance.ShowRewardedAd();
    }

    public void ReviveAward()
    {
        SoundManager.instance.PlayUiButtonSound();
        UIManager.instance.OnReviveClicked();
        PlayerHealth playerHealth =GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerHealth.Revived();
        playerHealth.AddHealth();
        GameManager.instance.Revived();
    }
}
