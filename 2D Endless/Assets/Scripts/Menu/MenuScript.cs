using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MenuScript : MonoBehaviour
{
    SoundManager soundManager;
    [Header("Quit")]
    [SerializeField] GameObject QuitCanvas;

    [Header("Settings")]
    [SerializeField] GameObject SettingsCanvas;
    [SerializeField] Slider musicSwitch;
    [SerializeField] Image musicStatusIcon;
    [SerializeField] Sprite musicOnSprite, musiCoffSprite;
    private const string MUSIC_PREF = "MUSIC_ENABLED";


    [SerializeField] Slider sfxSwitch;
    [SerializeField] Image sfxStatusIcon;
    [SerializeField] Sprite sfxOnSprite, sfxoffSprite;
    private const string SFX_PREF = "SFX_ENABLED";

    [Header("Name")]
    [SerializeField] GameObject NameCanvas;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] Button SetNameButton;
    [SerializeField] GameObject leaderBoardCanvas;


    [Header("Ads")]
    [SerializeField] GameObject AdsRemoveButton;
    [SerializeField] GameObject AdsConfirmcanvas;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        soundManager = SoundManager.instance;
        DisplayMusic();
        DisplaySFX();

        if (!PlayerPrefs.HasKey("UserName"))
        {
            ShowNameWindow();

        }
        CheckAdsStatus();
    }

    #region Exit Logic
    public void QuitWarning(bool _state)
    {
        soundManager.PlayUiButtonSound();
        QuitCanvas.SetActive(_state);
    }

    public void Quitgame()
    {
        soundManager.PlayUiButtonSound();
        Application.Quit();
    }
    #endregion


    #region Settings

    public void ShowSettings(bool _state)
    {
        soundManager.PlayUiButtonSound();
        SettingsCanvas.SetActive(_state);
    }

    public void ToggelMusic()
    {
        soundManager.ToggleMusic();
        DisplayMusic();
    }

    public void ToggleSfX()
    {
        soundManager.ToggleSFX();
        DisplaySFX();
    }

    void DisplayMusic()
    {
        bool musicOn = PlayerPrefs.GetInt(MUSIC_PREF, 1) == 1;
        if (musicOn)
        {
            musicSwitch.value = 1;
            musicStatusIcon.sprite = musicOnSprite;
        }
        else
        {
            musicStatusIcon.sprite = musiCoffSprite;

            musicSwitch.value = 0;
        }
    }

    void DisplaySFX()
    {
        bool sfxOn = PlayerPrefs.GetInt(SFX_PREF, 1) == 1;
        if (sfxOn)
        {
            sfxSwitch.value = 1;
            sfxStatusIcon.sprite = musicOnSprite;
        }
        else
        {
            sfxStatusIcon.sprite = musiCoffSprite;

            sfxSwitch.value = 0;
        }
    }
    #endregion


    #region name
    
    public void ShowNameWindow()
    {
        soundManager.PlayUiButtonSound();
        NameCanvas.SetActive(true);
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("UserName")))
        {
            nameInput.placeholder.GetComponent<TMP_Text>().text = "Write your name here.....";
        }
        else
        {
            nameInput.text = PlayerPrefs.GetString("UserName");
        }
    }

    public void CheckNameAvailibitlity()
    {
        if (nameInput.text.Length >= 3)
        {
            SetNameButton.interactable = true;

        }
        else
        {
            SetNameButton.interactable = false;
        }
    }

    public void SetName()
    {
        PlayerPrefs.SetString("UserName", nameInput.text);
        NameCanvas.SetActive(false);

    }

    #endregion


    #region leaderBoard

    public void ShowLeaderBoard(bool _active)
    {
        soundManager.PlayUiButtonSound();
        leaderBoardCanvas.SetActive(_active);
    }

    #endregion


    #region Ads

    public void CheckAdsStatus()
    {
        if (PlayerPrefs.GetInt("AdsRemoved") == 1)
            AdsRemoveButton.SetActive(false);
    }


    public void AdsConfirm(bool _state)
    {
        soundManager.PlayUiButtonSound();
        AdsConfirmcanvas.SetActive(_state);
    }

    public void BuyAds()
    {
        soundManager.PlayUiButtonSound();
        PlayerPrefs.SetInt("AdsRemoved",1);
        AdsConfirmcanvas.SetActive(false);
        CheckAdsStatus();
    }

    #endregion
}
