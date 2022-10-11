using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class QualityManager : MonoBehaviour
{
    enum RESOLUTIONS {
        _640_480,
        _1280_720,
        _1920_1080
    }
    
    public TMP_Dropdown ResDropdown;
    public TMP_Dropdown QualityDropdown;
    
    RESOLUTIONS resolution;
    void Start(){
        LoadSettings();
    }
    public void SaveSettings(bool apply = false){
        resolution = (RESOLUTIONS)ResDropdown.value;
        string stringRes = resolution.ToString().Substring(1,resolution.ToString().Length-1);
        int quality = QualityDropdown.value;
        PlayerPrefs.SetString("res",stringRes);
        PlayerPrefs.SetInt("quality",quality);
        PlayerPrefs.Save();
        if (apply){
            string loadedRes = PlayerPrefs.GetString("res");
            int loadedResX = int.Parse(loadedRes.Split('_')[0]);
            int loadedResY = int.Parse(loadedRes.Split('_')[1]);
            ApplySettings(loadedResX, loadedResY, quality );

        }
    }
    public void ApplySettings(int resX, int resY, int quality){
        // 1: low
        // 2: medium
        // 3: high
        // Apply Settings
        Debug.Log("applying settings");
        Screen.SetResolution(resX, resY, true);
        QualitySettings.SetQualityLevel(quality);
    }
    public void DiscardSettings(){
        LoadSettings();
    }
    public void LoadSettings(){
        // load from prefs
        // update ui
        
        string loadedRes = PlayerPrefs.GetString("res");
        int quality = PlayerPrefs.GetInt("quality");
        if(string.IsNullOrEmpty(loadedRes)){
          ResDropdown.value = 1;
          QualityDropdown.value = 1;
          SaveSettings();
          ApplySettings(1280, 720, 2);
        }else{
            try {
                int loadedResX = int.Parse(loadedRes.Split('_')[0]);
                int loadedResY = int.Parse(loadedRes.Split('_')[1]);
                ResDropdown.value = (int)System.Enum.Parse(typeof(RESOLUTIONS), "_"+loadedRes);
                QualityDropdown.value = quality;
                ApplySettings(loadedResX, loadedResY, quality+1);
            }
            catch {
                Debug.Log("error occured when loading settings");
            }
        }

        // float loadedQuality = PlayerPrefs.GetFloat
    }
    
}
