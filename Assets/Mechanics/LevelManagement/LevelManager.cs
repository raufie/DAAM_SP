using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject [] Enemies;
    private DifficultyManager difficultyManager;
    private EnemyManager enemyManager;
    public LevelParser levelParser;
    public GameObject Player;
    public CollectiblesManager HealthPickups;
    public CollectiblesManager AmmoPickups;
    // deeply internal state
    private float LoadingStartTime;
    private float StandbyFactor = 0.2f;
    private bool isLoaded = false;
    // ORDER OF LOADS
    // 1. Load Difficulties

    // 2. Load Enemies
    // 3 preparation of the levelData in terms of enemies for saving
    void Start(){
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        levelParser = GetComponent<LevelParser>();

        levelParser.LoadLevel();

        enemyManager = GetComponent<EnemyManager>();

        difficultyManager = GetComponent<DifficultyManager>();
        // LOADING DIFFICULTIES
        difficultyManager.SetDifficultyProfiles(levelParser.data.difficulty);
        // loading enemies

        // // NOW START THE CYCLE for loading enemies
        LoadingStartTime = Time.time;
        enemyManager.LoadEnemiesFromCode();
        LoadPlayer();       
        LoadEnemies();
        LoadWeapons();
        LoadCollectibles();
        LoadMilestone();
    }
    void Update(){
   
    }
    
    public void SetEnemies(){
        // set enemies to be saved
        
        levelParser.data.enemies = enemyManager.GetEnemiesCode();
        // levelParser.SetEnemyCode(enemyManager.GetEnemiesCode());
        LevelData.EntityData [] enemyData = new LevelData.EntityData[Enemies.Length];
        for (int i = 0 ; i < Enemies.Length;i++){

            if(Enemies[i]!= null){
                enemyData[i] = new LevelData.EntityData();
                enemyData[i].position = Enemies[i].GetComponent<Transform>().localPosition;
                enemyData[i].rotation = Enemies[i].transform.rotation.eulerAngles;
                enemyData[i].health = Enemies[i].GetComponent<EnemyBase>().healthPoints;
            }
        }
        levelParser.SetEnemyData(enemyData);

    }
    public void LoadPlayer(){
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        
        LevelData.EntityData playerData = levelParser.data.playerData;
        if(playerData.position == default(Vector3) || playerData.rotation == default(Vector3)){
            return;
        }
        if(playerData.position != default(Vector3)){
            Player.transform.localPosition = playerData.position;
            Player.transform.rotation = Quaternion.Euler(playerData.rotation);
            Player.GetComponent<PlayerObject>().Health = playerData.health;
        }
    }
    public void SetPlayer(){
        // set the level parser data
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        LevelData.EntityData playerData = new LevelData.EntityData();
        playerData.position = Player.transform.localPosition;
        playerData.rotation = Player.transform.rotation.eulerAngles;
        playerData.health = Player.GetComponent<PlayerObject>().Health;
        levelParser.SetPlayerData(playerData);
    }
    public void LoadEnemies(){
        LevelData.EntityData [] enemiesData = levelParser.data.enemyData;
        if(enemiesData == null){
            return;
        }
        for(int i = 0; i < enemiesData.Length;i++){
        
            if(enemiesData[i].position != default(Vector3)){
                Enemies[i].transform.parent.gameObject.SetActive(false);
                Enemies[i].SetActive(false);
                Enemies[i].transform.localPosition = levelParser.data.enemyData[i].position;    
                Enemies[i].transform.parent.gameObject.SetActive(true);
                Enemies[i].SetActive(true);
            }
            
        }
    }
    // WEAPONS
    public void LoadWeapons(){
        if (levelParser.data.weapons == null){
            return;
        }
        Debug.Log(levelParser.data.weapons.Length);
        Player = GameObject.FindGameObjectsWithTag("Player")[0];

        LevelData.WeaponData [] weapons = levelParser.data.weapons;
        for(int i = 0; i < weapons.Length;i++){
            Player.GetComponent<WeaponsManager>().weapons[i].currentLoadedAmmo = weapons[i].Magazine;
            Player.GetComponent<WeaponsManager>().weapons[i].currentNonLoadedAmmo = weapons[i].Remaining;
        }
        Player.GetComponent<WeaponsManager>().SwitchWeapon(levelParser.data.CurrentWeapon);
    }
    public void SetWeapons(){
        WeaponBase [] weapons = Player.GetComponent<WeaponsManager>().weapons;
        int currWeapon =(int) Player.GetComponent<WeaponsManager>().currentWeapon;
        LevelData.WeaponData [] weaponData = new LevelData.WeaponData[weapons.Length];
        for(int i = 0; i < weapons.Length; i++){
            weaponData[i] =  new LevelData.WeaponData();

            weaponData[i].Magazine = Player.GetComponent<WeaponsManager>().weapons[i].currentLoadedAmmo;
            weaponData[i].Remaining = Player.GetComponent<WeaponsManager>().weapons[i].currentNonLoadedAmmo;
        }
        levelParser.SetWeaponsData(weaponData, currWeapon);
    }
    public void LoadCollectibles(){
        if(levelParser.data.HealthPacks == null || levelParser.data.AmmoPacks == null){
            return;
        }
        HealthPickups.LoadFromCode(levelParser.data.HealthPacks);
        AmmoPickups.LoadFromCode(levelParser.data.AmmoPacks);
    }
    public void SetCollectibles(){
        string healthCode = HealthPickups.GetCode();
        string ammoCode = AmmoPickups.GetCode();
        levelParser.SetCollectibles(healthCode, ammoCode);
    }
    public void SetMilestone(){
        int currMilestone = GetComponent<MilestonesManager>().CurrentMilestone;
        levelParser.SetMilestone(currMilestone);
    }
    public void LoadMilestone(){

        MilestonesManager  milestonesManager = GetComponent<MilestonesManager>();
        
        milestonesManager.SwitchToMilestone(levelParser.data.currentMilestone);
    }

}
