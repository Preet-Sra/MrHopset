using TMPro;
using UnityEngine;

public class IAPManager : MonoBehaviour
{
    public static IAPManager instance;
    [SerializeField] GameObject InApWindow;
    [SerializeField] TMP_Text coinsText;
    AudioSource _audio;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ShowCoins();
    }

    public void ShowInAppWindow(bool _state)
    {
        SoundManager.instance.PlayUiButtonSound();
        InApWindow.SetActive(_state);
    }


    public void ConfirmPurchase(int _amt)
    {

        SoundManager.instance.PlayAudio(_audio);
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + _amt);
        coinsText.text = _amt.ToString();
        ShowInAppWindow(false);
    }

    public void ShowCoins()
    {
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString();

    }
}
