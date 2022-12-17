using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    public Toggle PermToggle;
    public AudioMixer mixer;
    private void Awake() {
        // if (canUse) {
            if (PlayerPrefs.HasKey("MasterVolume")) {
                float _MasterVolume = PlayerPrefs.GetFloat("MasterVolume");
                mixer.SetFloat("Volume", _MasterVolume);
                Debug.Log("MasterVolume loaded from PlayerPrefs as :" + _MasterVolume);
            }
            else{Debug.Log("MasterVolume was not stored in PlayerPrefs");}
            if (PlayerPrefs.HasKey("MusicVolume")) {
                float _MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
                Debug.Log("MusicVolume loaded from PlayerPrefs as :" + _MusicVolume);
            }
            else{Debug.Log("MusicVolume was not stored in PlayerPrefs");}

            if (PlayerPrefs.HasKey("SFXVolume")) {
                float _SFXVolume = PlayerPrefs.GetFloat("SFXVolume");
                Debug.Log("SFXVolume loaded from PlayerPrefs as :" + _SFXVolume);
            }
            else{Debug.Log("SFXVolume was not stored in PlayerPrefs");}

            if (PlayerPrefs.HasKey("PermDeath")) {
                int _perm_death= PlayerPrefs.GetInt("PermDeath");
                Debug.Log("PermDeath loaded from PlayerPrefs as :" + _perm_death);
                if (_perm_death == 1){
                    PermToggle.isOn=true;
                } else {
                    PermToggle.isOn=false;
                }
            }
            else{Debug.Log("PermDeath was not stored in PlayerPrefs");}
            // repeat for every other setting?
        // }
    }
}
