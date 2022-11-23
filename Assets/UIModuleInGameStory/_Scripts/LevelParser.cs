using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelParser : MonoBehaviour
{
    // ONLY PURPOSE IS TO GENERATE a LevelData instance based on the level information
    // Start is called before the first frame update
    public LevelData data;
    
    void Awake(){
        data = new LevelData();
    }
    public LevelData Save(string saveName){
        // SAVE BASIC INFORMATION
        
        data.chapter = SceneManager.GetActiveScene().buildIndex;
        data.name= saveName;
        data.date= System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
        data.isNew = false;
        return data;
    }
    public static void SaveAsCurrent(){

    }
    public LevelData LoadLevel(){
        data = StateManager.LoadCurrent();        
        return data;
    }
    public void SetEnemyCode(string s){
        data.enemies = s;
    }
    public void SetEnemyData(LevelData.EntityData [] _enemyData){
        data.enemyData = _enemyData;
    }
    public void SetPlayerData(LevelData.EntityData _playerData){
        data.playerData = _playerData;
    }
    public void SetWeaponsData(LevelData.WeaponData [] _weaponData, int currentData){
        data.weapons = _weaponData;
        data.CurrentWeapon = currentData;
    }
    public void SetCollectibles(string health, string ammo){
        data.HealthPacks = health;
        data.AmmoPacks = ammo;
    }
    public void SetMilestone(int milestone){
        data.currentMilestone = milestone;
    }
    public void SetDifficulty(int difficulty){
        if(difficulty >=0 && difficulty <=2 ){
            data.difficulty = difficulty;
        }
    }

}
