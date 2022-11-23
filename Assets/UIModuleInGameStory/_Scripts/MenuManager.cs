using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    // Menu manager should connect buttons to their behavior

    public Button SaveBtn;
    public TMP_InputField Input;
    public LevelManager levelManager;
    // Milestones
    public HUDManager hudManager;
    public TMP_Text MilestoneLabel;
    public TMP_Text MilestoneDescription;
    // Changing difficulty
    public TMP_Dropdown DifficultyDropdown;
    public Button DifficultyBtn;
    // QUIT TI menu
    public Button QuitToMenu;

    // Quit to menu
    void Start(){
        AddListeners();
        DifficultyDropdown.value = levelManager.levelParser.data.difficulty;
        
    } 
    void LateUpdate(){
        MilestoneLabel.text = hudManager.info_label.text;
        MilestoneDescription.text = hudManager.info_desc.text;
    }
    void AddListeners(){
        // add listenrers
        SaveBtn.onClick.AddListener(()=>{
            // Save ...
            SaveGame(Input.text);
        });
        DifficultyBtn.onClick.AddListener(()=>{
            levelManager.difficultyManager.SetDifficultyProfiles(DifficultyDropdown.value);
        });
        QuitToMenu.onClick.AddListener(()=>{
            SceneManager.LoadScene(0);
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
        levelManager.SetDifficulty(DifficultyDropdown.value);

        // Call the stateManager's static SaveLevelData function
        levelManager.gameObject.GetComponent<NotificationsManager>().ShowAlert("Saving Game", "", Notification.NotificationType.LOADING, 10);
        StateManager.SaveLevelData(data, fileName);
        levelManager.gameObject.GetComponent<NotificationsManager>().ShowAlert("Game Saved Successfully", "", Notification.NotificationType.SUCCESS, 2);


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
