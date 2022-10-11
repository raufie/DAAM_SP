using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{   
    [System.Serializable]
    public struct EntityData {
        public Vector3 position;
        public Vector3 rotation;
        public int health;
    }
    [System.Serializable]
    public struct WeaponData {
        public int Magazine;
        public int Remaining;
    }
    // BASIC LEVEL INFORMATION
    public int chapter;
    public string name;
    public string date;
    public string path;
    public string enemies;
    public bool isNew;
    public int difficulty;
    public EntityData [] enemyData;
    public EntityData playerData;
    // WEAPONS
    public WeaponData [] weapons;
    public int CurrentWeapon;
    // HEALTH AND AMMO PACKS
    public string HealthPacks;
    public string AmmoPacks;
    // Milestones
    public int currentMilestone;
    // 0,1,2

    public void LoadFromJson(string json){
        JsonUtility.FromJsonOverwrite(json, this);
        
    }  
    public string ToJson(){
        return JsonUtility.ToJson(this);
    }

}
