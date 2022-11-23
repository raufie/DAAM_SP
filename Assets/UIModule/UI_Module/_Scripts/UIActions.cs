using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActions : MonoBehaviour
{
    public Rebinding rebinding;
    public QualityManager qualityManager;

    public void saveAudioSettings(float master, float sfx, float music){
        PlayerPrefs.SetFloat("masterVolume", master);
        PlayerPrefs.SetFloat("sfxVolume", sfx);
        PlayerPrefs.SetFloat("bgmVolume", music);
        PlayerPrefs.Save();
    }
    public void saveControlSettings(){
        rebinding.SaveBindings();
    }
    public void discardControlSettings(){
        rebinding.ResetBindings();
    }
    public void saveVideoSettings(){
        qualityManager.SaveSettings(true);   
    }
    public void discardVideoSettings(){
        qualityManager.DiscardSettings();   
    }
}
