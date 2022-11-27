using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;
using UnityEngine.InputSystem;
using Cinemachine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;
public class NetworkPlayerObject : NetworkBehaviour
{   
    public string Username;
    public int Kills;
    public int Deaths;
    public int Team = 1;
    [SyncVar]
    public string ID;
    private int MODEL;
    public GameObject []SpawnPoints;
    public int SpawnPoint;
    // tmps
    public float StartTime;
    private InputMaster controls;
    public GameObject [] Models;
    public GameObject PlayerObject;
    public int TESTMODEL;
    public CinemachineVirtualCamera Cam;
    public Material Team1Mat;
    public Material Team2Mat;
    public GameObject ShirtObject;
    
    void Awake(){
        if(isLocalPlayer){
        ID = Guid.NewGuid().ToString();
        controls = new InputMaster();
        }
        
    }
    void OnEnable(){
        if(isLocalPlayer){
        if(controls == null){
            controls = new InputMaster();
        }
        controls.Enable();
        }
        
        
    }
    void OnDisable(){

    }
    void Start(){
        if(isLocalPlayer){
            string email = PlayerPrefs.GetString("email");
            string password = PlayerPrefs.GetString("password");
            // Debug.Log(mail);
            if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password)){
                var request = new LoginWithEmailAddressRequest {
                    Email  = email,
                    Password = password
                };
                PlayFabClientAPI.LoginWithEmailAddress(request, res=>{},
                error=>{}
                );
            }
            SpawnPoints = new GameObject[6]{
                GameObject.FindGameObjectsWithTag("p1")[0],
                GameObject.FindGameObjectsWithTag("p2")[0],
                GameObject.FindGameObjectsWithTag("p3")[0],
                GameObject.FindGameObjectsWithTag("p4")[0],
                GameObject.FindGameObjectsWithTag("p5")[0],
                GameObject.FindGameObjectsWithTag("p6")[0]
            };

            MODEL = PlayerPrefs.GetInt("model");
            Username = PlayerPrefs.GetString("username");
            Username ="bravo1234";
            // MODEL = TESTMODEL;

            ID = Guid.NewGuid().ToString();
            Debug.Log(ID);
            Team = 1;
            Kills = 0;
            Deaths = 0;

            CmdGetStartTime();
            
            CmdAddPlayerEntry(Username,ID,MODEL, (int)netId);
        }
    }
    void Update(){
        if(isLocalPlayer){
        // CmdAddKill();
            if(controls == null){
                controls = new InputMaster();
                controls.Enable();
            }
            // controls.Player.Shoot.performed+=(ctx)=>{
            //     CmdAddKill(Team, ID);
            // };
            // controls.Player.Jump.performed+=(ctx)=>{
            //     CmdAddDeath(ID);
            // };
        }
    }
    void FixedUpdate(){
        if(isLocalPlayer){
            MPSpecs specs = GameObject.FindGameObjectsWithTag("MPSpecs")[0].GetComponent<MPSpecs>();
            float remainingTime = specs.GameTime*60 - ((float)NetworkTime.time-StartTime);
            GameObject.FindGameObjectsWithTag("HudManager")[0].GetComponent<MPHudManager>().UpdateTime(remainingTime );

            if(remainingTime <=0f){
                // LEAVE GAME AND SWITCH SCENE
                // NetworkClient.StopClient();
                GameObject.FindGameObjectWithTag("InputManagement").GetComponent<MPInputManagement>().disable();
                GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<MyNetworkManager>().StopClient();
                SceneManager.LoadScene(16);
            }
        
        }
        
    }
    [Command]
    public void CmdUpdatePlayerInfo(string username,int team, int id, int kills, int deaths  ){
       
    }
    [Command]
    public void CmdAddPlayerEntry(string username, string id, int model, int NetID){
        // add a new player and get the team for that player (basically balances teams to have equal on both sides(at start only))
        GameManager gameManager = GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<GameManager>();
        int team = gameManager.AddPlayer(username, id, model, NetID);
        int index = gameManager.GetIndex(id);
        if(team == -1){
            return;
        }
        SpawnById(id, index);
        UpdatePlayer(team, index);

        // int modelIndex = gameManager.GetModel(id);
        // SwitchToModel(id, modelIndex);
        PlayerEntry[] Players = gameManager.GetPlayers();
        // UpdateModels(Players);
        UpdateClientTable(Players);
    }
    [Command]
    public void CmdAddKill(int team, string id, GameObject killer, GameObject dead){
        
        GameManager gameManager = GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<GameManager>();
        team = gameManager.GetTeam(id);
        gameManager.AddKill(team, id);
        UpdateClientUI(gameManager.Team1Score, gameManager.Team2Score);

        int index = gameManager.GetIndex(id);
        if(Team == -1){
            return;
        }
        int kills = gameManager.GetKills(id);
        int deaths = gameManager.GetDeaths(id);
        string killerUsername = gameManager.GetUsername(id);
        Debug.Log(dead.GetComponent<NetworkPlayerObject>().ID);
        string deadUsernamme = gameManager.GetUsernameByNetID((int)dead.GetComponent<NetworkPlayerObject>().netId);
        // string dead = gameManager.GetUsername(deadId);
        UpdatePlayer(team, index, kills, deaths);       
        PushClientKillUpdate(killerUsername, deadUsernamme );       

        UpdatePrefsScore(gameManager.Team1Score, gameManager.Team2Score);
    }
    [ClientRpc]
    public void UpdatePrefsScore(int t1 ,int t2){
        PlayerPrefs.SetInt("t1score", t1);
        PlayerPrefs.SetInt("t2score", t2);
    }
    [Command]
    public void CmdAddDeath(string id){
        GameManager gameManager = GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<GameManager>();
        gameManager.AddDeath(id);
        int index = gameManager.GetIndex(id);
        int kills = gameManager.GetKills(id);
        int deaths = gameManager.GetDeaths(id);

        UpdatePlayer(Team, index, kills, deaths);
    }
    [Command]
    public void CmdGetStartTime(){
        GameManager gameManager = GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<GameManager>();
        float startTime =(float) gameManager.manager.StartTime;
        SetStartTime(startTime);
    }
    [Command]
    public void CmdSpawnPlayer(GameObject target, string id){
        NetworkIdentity NetId = target.GetComponent<NetworkIdentity>();

        GameManager gameManager = GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<GameManager>();
        int index = gameManager.GetIndex(id);
        TargetSpawnPlayer(NetId.connectionToClient, index);
    }
    // CLIENT RPCS
    [ClientRpc]
    public void UpdatePlayer(int team, int index){
        Team = team;
        GameObject.FindGameObjectsWithTag("HudManager")[0].GetComponent<MPHudManager>().UpdateInfoEntry(Username, Kills, Deaths, index);
        GameObject.FindGameObjectsWithTag("HudManager")[0].GetComponent<MPHudManager>().UpdatePlayerObject();
        GameObject.FindGameObjectsWithTag("InputManagement")[0].GetComponent<MPMovementInput>().enabled =true;
        GameObject.FindGameObjectsWithTag("InputManagement")[0].GetComponent<MPWeaponsInput>().enabled =true;
        GameObject.FindGameObjectsWithTag("InputManagement")[0].GetComponent<MPConfig>().enabled =true;
        // update material
        if(team == 0){
        ShirtObject.GetComponent<SkinnedMeshRenderer>().material = Team1Mat;
        }else{
        ShirtObject.GetComponent<SkinnedMeshRenderer>().material = Team2Mat;
        }
    }
    [ClientRpc]
    public void UpdatePlayer(int team, int index, int kills, int deaths ){
        Team = team;
        GameObject.FindGameObjectsWithTag("HudManager")[0].GetComponent<MPHudManager>().UpdateInfoEntry(Username, kills, deaths, index);
    }
    [ClientRpc]
    public void UpdateClientTable(PlayerEntry[] players){
        for(int i = 0; i < players.Length;i++){
            if(string.IsNullOrEmpty(players[i].username)){
                continue;
            }
        GameObject.FindGameObjectsWithTag("HudManager")[0].GetComponent<MPHudManager>().UpdateInfoEntry(players[i].username, players[i].kills,players[i].deaths, i); 
        }        
    }

    [ClientRpc]
    public void PushClientKillUpdate(string killer, string dead){
        if(killer == null || dead == null){
            return;
        } 
        GameObject.FindGameObjectsWithTag("HudManager")[0].GetComponent<KillUpdateManager>().PushKill(killer, dead);
    }
    [ClientRpc]
    public void UpdateClientUI(int score1, int score2){
        GameObject.FindGameObjectsWithTag("HudManager")[0].GetComponent<MPHudManager>().UpdateScore(score1, score2);
    }
    
    [ClientRpc]
    public void SetStartTime(float t){
        StartTime = t;
    } 
    // TARGET RPCS
    [TargetRpc]
    public void TargetSpawnPlayer(NetworkConnection target, int index){

        transform.position = SpawnPoints[index].transform.position;
    }
    [ClientRpc]
    public void SpawnById(string _id, int index){
        if (ID == _id){
            transform.position = SpawnPoints[index].transform.position;
        }
        SpawnPoint = index;
    }
    [ClientRpc]
    public void SwitchToModel(string _id, int modelIndex){
        Debug.Log(ID);
        Debug.Log(_id);
        Debug.Log(modelIndex);
        if(ID != _id){
            return;
        }
        for(int i = 0 ; i < Models.Length;i++){
            if(i == modelIndex){
                Models[i].SetActive(true);
            }else{
                Models[i].SetActive(false);
            }
        }
    }
    public void ChangeModel(int model){
        for(int i = 0 ; i < Models.Length;i++){
            if(i == model){
                Models[i].SetActive(true);
            }else{
                Models[i].SetActive(false);
            }
        }
    }
    [ClientRpc]
    public void UpdateModels(ModelItem[] Players){
        GameObject [] Objects = GameObject.FindGameObjectsWithTag("PlayerWrapper");
        for(int i = 0; i < Objects.Length;i++){
            for(int j = 0; j < Players.Length;j++){
                if(Objects[i].GetComponent<NetworkPlayerObject>().ID == Players[j].id){
                    Objects[i].GetComponent<NetworkPlayerObject>().UpdateCharacter(Players[j].model);
                }
            }
        }
    }
    public void UpdateCharacter(int model){
        ChangeModel(model);
    }
    // TARGET RPC
    [Command]
    public void CmdDamage(GameObject player, int damage){
        // player.GetComponent<NetworkTelemetaryObject>().takeDamage(damage);
        NetworkIdentity identity = player.GetComponent<NetworkIdentity>();
        GameManager gameManager = GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<GameManager>();
        NetworkTelemetaryObject tele = player.GetComponent<NetworkTelemetaryObject>();
        int index = gameManager.GetIndex(player.GetComponent<NetworkPlayerObject>().ID);
        if (tele.Health <= 0)
        {
            // Destroy(gameObject);
            
            tele.KillPlayer();
            tele.Health = 100;
            
        }
        else if(tele.Health - damage <0){
            tele.Health = 0;
        }
        else{
            tele.Health -= damage;
        }
        TargetTakeDamage(identity.connectionToClient, damage,  index);
        
    }
    [TargetRpc]
    public void TargetTakeDamage(NetworkConnection target, int damage, int index){
        NetworkClient.localPlayer.gameObject.GetComponent<NetworkTelemetaryObject>().takeDamage(damage);
        // GetComponent<NetworkTelemetaryObject>().takeDamage(damage);
        if(NetworkClient.localPlayer.gameObject.GetComponent<NetworkTelemetaryObject>().Health == 0){
            Debug.Log("im dead yo");
            NetworkClient.localPlayer.gameObject.GetComponent<NetworkTelemetaryObject>().KillPlayer();
            // SpawnById(ID, index);
        
            // NetworkClient.localPlayer.gameObject.GetComponent<NetworkTelemetaryObject>().EnableRagdoll();
        
        }
    }
    
}
