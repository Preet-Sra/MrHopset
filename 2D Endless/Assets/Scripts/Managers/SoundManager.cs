using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioSource sfxAudio;
    [SerializeField] AudioSource bgMusicSource;
    [SerializeField] AudioClip uiButtonPress;
    [SerializeField] float bgMusicVolume = 0.5f;
    [SerializeField] float sfxVolume = 1f;

    private const string SFX_PREF = "SFX_ENABLED";
    private const string MUSIC_PREF = "MUSIC_ENABLED";
    bool sfxOn;
    bool musicOn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSoundPreferences();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayAudio(AudioSource _audiosource)
    {
        if (_audiosource != null && _audiosource.clip != null)
            sfxAudio.PlayOneShot(_audiosource.clip);
    }

    public void PlayAudio(AudioClip _clip)
    {
        if (_clip != null)
            sfxAudio.PlayOneShot(_clip);
    }

    public void PlayUiButtonSound()
    {
        if (uiButtonPress != null)
            sfxAudio.PlayOneShot(uiButtonPress);
    }

    public void ToggleSFX()
    {
        sfxOn = !sfxOn;
        PlayerPrefs.SetInt(SFX_PREF, sfxOn? 1 : 0);
        PlayerPrefs.Save();
        sfxAudio.volume = sfxOn ? sfxVolume : 0f;
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;
        PlayerPrefs.SetInt(MUSIC_PREF, musicOn ? 1 : 0);
        PlayerPrefs.Save();
        bgMusicSource.volume = musicOn ? bgMusicVolume : 0f;
    }

    private void LoadSoundPreferences()
    {
        sfxOn = PlayerPrefs.GetInt(SFX_PREF, 1) == 1;
        musicOn = PlayerPrefs.GetInt(MUSIC_PREF, 1) == 1;

        sfxAudio.volume = sfxOn ? sfxVolume : 0f;
        bgMusicSource.volume = musicOn ? bgMusicVolume : 0f;
    }
    public float GetSFXVolume()
    {
        return sfxAudio.volume;
    }
}
