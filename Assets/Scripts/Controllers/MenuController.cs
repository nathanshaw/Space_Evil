using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    public AudioListener master;
    public AudioListener sfx;
    public AudioListener music;

    // Allows the User to Choose what Level to Load...
    // [Header("Levels to Load")]
    // public string _newGameLevel;
    // private string levelToLoad;

    [Header("Gameplay")]
    public int perm_death;

    [Header("Audio")]
    [SerializeField] private TMP_Text MasterVolumeTextValue = null;
    [SerializeField] private Slider MasterVolumeSlider = null;
    [SerializeField] private float MasterVolume;

    [SerializeField] private TMP_Text SFXVolumeTextValue = null;
    [SerializeField] private Slider SFXVolumeSlider = null;
    [SerializeField] private float SFXVolume = 0.8f;

    [SerializeField] private TMP_Text MusicVolumeTextValue = null;
    [SerializeField] private Slider MusicVolumeSlider = null;
    [SerializeField] private float MusicVolume = 0.8f;

    // initalise player settings from the PlayerPref's class =)
    public void Start() {
        perm_death = PlayerPrefs.GetInt("PermDeath", 0);
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
        MusicVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        SFXVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////// Gameplay Section ///////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    public void ApplyGameplayOptions()
    {
        // TODO - also add setting for difficulty?
        ApplyPermDeath();
        PlayerPrefs.Save();
        Debug.Log("set permDeath to " + perm_death + PlayerPrefs.GetInt("PermDeath"));
    }
    public void SetPermDeath(int val)
    {
        perm_death = val;
    }
    private void ApplyPermDeath()
    {
        PlayerPrefs.SetInt("PermDeath", perm_death);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////// Gameplay Section ///////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////
    public void ApplyAudioSettings()
    {
        ApplyMasterVolume();
        ApplyMusicVolume();
        ApplySFXVolume();
        PlayerPrefs.Save();
        Debug.Log("Master Volume applied at " +  PlayerPrefs.GetFloat("MasterVolume"));
        Debug.Log("Music Volume applied at " +  PlayerPrefs.GetFloat("MusicVolume"));
        Debug.Log("SFX Volume applied at" + PlayerPrefs.GetFloat("SFXVolume"));
    }
    public void SetMasterVolume()
    {
        Debug.Log("Master Volume should be set to" + MasterVolumeSlider.value);
        MasterVolume = MasterVolumeSlider.value;
        MasterVolumeTextValue.text = MasterVolume.ToString("0.00");
        master.Volume
        Debug.Log("Master Volume set to" + MasterVolume);
    }

    public void SetSFXVolume(float vol)
    {
        SFXVolume = vol;
        SFXVolumeTextValue.text = vol.ToString();
        Debug.Log("SFX Volume set to" + SFXVolume);
    }
    public void SetMusicVolume(float vol)
    {
        MusicVolume = vol;
        MusicVolumeTextValue.text = vol.ToString("0.00");
    }

    private void ApplyMasterVolume()
    {
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
    }
    private void ApplyMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
    }
    private void ApplySFXVolume()
    {
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
    }

    // TODO - load saved game
    public void LoadSavedGame()
    {
        if (PlayerPrefs.HasKey("SavedGame"))
        {
            // TODO - do something to load the saved data...
        }

    }

}

