using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
public class MPMenuManager : MonoBehaviour
{
    // Menu manager should connect buttons to their behavior


    // Milestones
    // QUIT TI menu
    public Button QuitToMenu;
    public Button QuitToWindows;
    public Toggle MapToggle;
    public GameObject MapObject;
    public int MPBASESCENE;
    // Quit to menu
    void Start(){
        AddListeners();
        
    } 
   
    void AddListeners(){

        QuitToMenu.onClick.AddListener(()=>{
            SceneManager.LoadScene(MPBASESCENE);
        });
        QuitToWindows.onClick.AddListener(()=>{
           Application.Quit();
        });
        MapToggle.onValueChanged.AddListener(delegate {
            ToggleMapDisplay(MapToggle.isOn);
        });
    }
    void ToggleMapDisplay(bool IsOn){
        if(IsOn){
            MapObject.SetActive(true);
        }else{
            MapObject.SetActive(false);
        }
    }

  
}
