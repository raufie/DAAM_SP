using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class MenuManager : MonoBehaviour
{
    // Menu manager should connect buttons to their behavior

    public Button SaveBtn;
    public TMP_InputField Input;
    public LevelManager levelManager;
    void Start(){
        AddListeners();
        
    } 
    void AddListeners(){
        // add listenrers
        SaveBtn.onClick.AddListener(()=>{
            // Save ...
            SaveGame(Input.text);
        });
    }
    void SaveGame(string input){
    
        // Create a UUID (a file name)
        RefreshLevelManager();
        Guid uuid = Guid.NewGuid();
        string uuidStr = uuid.ToString();
        string fileName = input+"-"+uuidStr+".dat";
        // Get LevelData from Level Parser (to get current level detail)
        // ONLY PASS THE INPUT to level Parser
        LevelData data = levelManager.levelParser.Save(input);
        // Call the stateManager's static SaveLevelData function
        StateManager.SaveLevelData(data, fileName);

        // 
    }
    void RefreshLevelManager(){
        levelManager.SetEnemies();
        levelManager.SetPlayer();
        levelManager.SetWeapons();
        levelManager.SetCollectibles();
        levelManager.SetMilestone();
    }
}
