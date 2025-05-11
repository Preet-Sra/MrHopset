using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Ricimi;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    SoundManager soundManager;

    [Header("Countdown")]
    float coutdownTime = 1f;
    [SerializeField] GameObject[] countdownItems;
    [SerializeField] GameObject startingCanvas;
    [SerializeField] AudioSource countdownAudio;

    [Header("Coins")]
    [SerializeField] TMP_Text coinsText;
    [SerializeField] float scaleFactor = 0.5f;
    public Transform coinsHolder;
    Vector3 coinsHolderStartScale;

    [Header("PowerUp")]
    [SerializeField] GameObject powerupCanvas;
    [SerializeField] Image powerupIcon;
    [SerializeField] Slider powerupTimer;


    [Header("Disatnce")]
    [SerializeField] TMP_Text distanceText;

    [Header("Revive UI")]
    [SerializeField] GameObject reviveCanvas;
    [SerializeField] TMP_Text reviveCounterText;
    [SerializeField] Image reviveFillImage;
    Coroutine reviveCoroutine;

    [Header("GameOver")]
    [SerializeField] AudioClip GameOverSound;
    bool reviveCanvasAlreadyShowed;
    [SerializeField] GameObject GameOverCanvas;
    [SerializeField] TMP_Text totalDistanceText;
    [SerializeField] TMP_Text highcsoreText;
    [SerializeField] TMP_Text TotalCoinsText;
    int coinsInThisGame;
    float distanceInThisGame;

    [Header("Pause")]
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] Slider musicSwitch;
    [SerializeField] Image musicStatusIcon;
    [SerializeField] Sprite musicOnSprite, musiCoffSprite;
    private const string MUSIC_PREF = "MUSIC_ENABLED";


    [SerializeField] Slider sfxSwitch;
    [SerializeField] Image sfxStatusIcon;
    [SerializeField] Sprite sfxOnSprite, sfxoffSprite;
    private const string SFX_PREF = "SFX_ENABLED";

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        ActivatePowerUp(false);
        soundManager = SoundManager.instance;
        coinsHolderStartScale = coinsHolder.transform.localScale;
        DisplayCoins(0);
        StartCoroutine(StartCountdown());
        DisplayMusic();
        DisplaySFX();
    }



    IEnumerator StartCountdown()
    {
        for (int i = 0; i < countdownItems.Length; i++)
        {
            GameObject go = countdownItems[i];
            go.SetActive(true);
            go.transform.localScale = Vector3.zero;

            go.transform.DOScale(1f, 0.15f).SetEase(Ease.OutBack);
            soundManager.PlayAudio(countdownAudio);
            yield return new WaitForSeconds(coutdownTime);

            go.SetActive(false);
        }
        GameManager.instance.StartGame();
        startingCanvas.SetActive(false);
    }

    public void DisplayDistance(float _distance)
    {
        _distance = Mathf.RoundToInt(_distance);
        distanceInThisGame = _distance;
        if (distanceInThisGame >= PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", distanceInThisGame);
            PlayerPrefs.Save();
        }

        distanceText.text = _distance + " m";
    }

    public void DisplayCoins(int _coins)
    {
        coinsInThisGame = _coins;
        coinsText.text = _coins.ToString();
        coinsHolder.transform.DOScale(coinsHolderStartScale + Vector3.one * scaleFactor, 0.2f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                coinsHolder.transform.DOScale(coinsHolderStartScale, 0.2f).SetEase(Ease.OutBounce);
            });
    }

    public void AddCoins(int amount)
    {
        coinsInThisGame += amount;
        DisplayCoins(amount);
    }

    public void GameOver()
    {
        if (reviveCoroutine != null) StopCoroutine(reviveCoroutine); 
        reviveCoroutine = StartCoroutine(GameOverSuquence());
    }

    IEnumerator GameOverSuquence()
    {
        if (!reviveCanvasAlreadyShowed)
        {
            reviveCanvasAlreadyShowed = true;

            reviveCanvas.SetActive(true);
            reviveFillImage.fillAmount = 1;
            float reviveTime = 5f;
            float timer = reviveTime;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                reviveCounterText.text = Mathf.CeilToInt(timer).ToString();
                reviveFillImage.fillAmount = timer / reviveTime;
                yield return null;
            }

            // Time's up and player didn't press revive
            reviveCanvas.SetActive(false);
            ShowGameOverCanvas();

        }
        else
        {
            ShowGameOverCanvas();
        }
    }

    public void OnReviveClicked()
    {
        
        reviveCanvas.SetActive(false);
        StopCoroutine(reviveCoroutine);
        // Add logic to continue gameplay, like resetting player state
        Debug.Log("Player revived!");
    }

    public void ShowGameOverCanvas()
    {
        soundManager.PlayAudio(GameOverSound);
        int overallCoins = coinsInThisGame + PlayerPrefs.GetInt("Coins");
        PlayerPrefs.SetInt("Coins", overallCoins);
        PlayerPrefs.Save();
        
        totalDistanceText.text = "Distance: " + distanceInThisGame +" m";
       
        highcsoreText.text = "HighScore: " + PlayerPrefs.GetFloat("HighScore") + " m";
        
        TotalCoinsText.text =overallCoins+"<sprite=0>";
        GameOverCanvas.SetActive(true);

        if (PlayerPrefs.GetInt("AdsRemoved") != 1)
        {
            AdmobAdsScript.instance.ShowInterstitialAd();
        }
    }

    public void MainMenu()
    {
        soundManager.PlayUiButtonSound();
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        soundManager.PlayUiButtonSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        soundManager.PlayUiButtonSound();
        Time.timeScale = 0;
        pauseCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        soundManager.PlayUiButtonSound();
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
    }

    public void ShowPowerUpTimer(Sprite _powerupIcon, float maxTime, float currentTime)
    {
        powerupIcon.sprite = _powerupIcon;
        powerupTimer.maxValue = maxTime;
        powerupTimer.value = currentTime;
    }

    public void ActivatePowerUp(bool _state)
    {
        powerupCanvas.SetActive(_state);
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
}
