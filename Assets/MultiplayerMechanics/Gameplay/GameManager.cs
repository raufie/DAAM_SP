using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
public struct ModelItem {
    public int model;
    public string id;
}
public struct PlayerEntry {
    public string username;
    public int team;
    public int kills;
    public int deaths;
    public string id;
    public int model;
    public int NetID;
}
public class GameManager : NetworkBehaviour
{


    [SyncVar(hook = nameof(ScoreChange))]
    public int Team1Score;
    [SyncVar(hook = nameof(ScoreChange))]
    public int Team2Score;
    // Hold All Player infos
    public readonly SyncList<PlayerEntry> Players = new SyncList<PlayerEntry>();
    public MyNetworkManager manager;
    public float GameTime = 10f;
    
    // Start is called before the first frame update

    void  FixedUpdate(){
         if(isServer){
            MPSpecs specs = GameObject.FindGameObjectsWithTag("MPSpecs")[0].GetComponent<MPSpecs>();
            MyNetworkManager manager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<MyNetworkManager>();
            float remainingTime = specs.GameTime*60 - ((float)NetworkTime.time-manager.StartTime);
            Debug.Log(remainingTime);
            if(remainingTime <=-5f){
                Debug.Log("TRYNNA KILL SERVER");
                // LEAVE GAME AND SWITCH SCENE
                manager.StopServer();
                Application.Quit();
                // SceneManager.LoadScene(16);
            }
        
        }
    }
    void Update(){
        // GetComponent<MyNetworkManager>().NetworkServer.connections;
    }
    [Command(requiresAuthority=false)]
    public void CmdAddKill(int team){
        Debug.Log("Adding Kill");
        if(team == 0){
            Team1Score +=1;
        }else{
            Team2Score += 1;
        }
    }
    

    public void ScoreChange(int oldVal, int newVal){
        PlayerPrefs.SetInt("t1score", Team1Score);
        PlayerPrefs.SetInt("t2score", Team2Score);
    }

    public void AddKill(int TeamInput, string id){
        int team = GetTeam(id);
        if(team == 0){
            Team1Score +=1;
        }else{
            Team2Score += 1;
        }
        for(int i = 0 ; i < Players.Count;i++){
            if(id == Players[i].id){
                PlayerEntry newPlayer = new PlayerEntry();
                newPlayer.username = Players[i].username;
                newPlayer.kills = Players[i].kills+1;
                newPlayer.deaths =Players[i].deaths;
                newPlayer.team = Players[i].team;
                newPlayer.id = Players[i].id;
                
                Players[i] = newPlayer;
            }
        }   
        
        // UpdateScores(Team1Score, Team2Score);
    }
    public int AddPlayer(string username, string id, int model , int NetID){
        int team0 = 0;
        int team1 = 0;
        int team;
        for(int i = 0 ; i < Players.Count;i++){
            // if(id == Players[i].id){
            //     return Players[i].team;
            // }
            if(Players[i].team == 0){
                team0++;
            }else{
                team1++;
            }
        }   
        Debug.Log("in team 0:"+team0);
        Debug.Log("in team 1:"+team1);
        if (team1 < team0){
            team = 1;
        }else{
            team = 0;
        }

        PlayerEntry player = new PlayerEntry();
        player.username = username;
        player.id = id;
        player.team = team;
        player.model = model;
        player.NetID = NetID;
        Players.Add(player);
        Debug.Log(Players[0].username);
        Debug.Log(Players[0].id);
        return team;
    }

    public void AddDeath(string id){
        
        for(int i = 0 ; i < Players.Count;i++){
            if(id == Players[i].id){
                PlayerEntry newPlayer = new PlayerEntry();
                newPlayer.username = Players[i].username;
                newPlayer.kills = Players[i].kills;
                newPlayer.deaths = Players[i].deaths+1;
                newPlayer.team = Players[i].team;
                newPlayer.id = Players[i].id;
                Players[i] = newPlayer;
            }
        }   
    }
    public int GetIndex(string id){
        for(int i = 0 ; i < Players.Count;i++){
            if(id == Players[i].id){
                return i;
            }
        }   
        return 0;
    }
    // change handlers
    [ClientRpc]
    public void UpdateScores(int score1, int score2){
        Debug.Log(score1+" - " + score2);
    }
    public void Team1Change(int old, int newVal){
        
        Debug.Log(""+newVal+" score");
    }
    public int GetKills(string id){
        for(int i = 0 ; i < Players.Count;i++){
            if(id == Players[i].id){
                int kills = Players[i].kills;
                // Debug.Log(kills);
                return kills;
            }
        }
        return 0;   
    }
    public int GetDeaths(string id){
        for(int i = 0 ; i < Players.Count;i++){
            if(id == Players[i].id){
                int deaths = Players[i].deaths;
                return deaths;
            }
        }
        return 0;   
    }
    public int GetTeam(string id){
        for(int i = 0 ; i < Players.Count;i++){
            if(id == Players[i].id){
                int team = Players[i].team;
                return team;
            }
        }
        return 0; 

    }
    public string GetUsername(string id){
        for(int i = 0 ; i < Players.Count;i++){
            if(id == Players[i].id){
                string username = Players[i].username;
                return username;
            }
        }
        return ""; 

    }
    public string GetUsernameByNetID(int NetID){
        for(int i = 0 ; i < Players.Count;i++){
            if(NetID == Players[i].NetID){
                string username = Players[i].username;
                return username;
            }
        }
        return ""; 

    }
    
    public int GetModel(string id){
        for(int i = 0 ; i < Players.Count;i++){
            if(id == Players[i].id){
                int model = Players[i].model;
                return model;
            }
        }
        return 0; 
    }   
    // public void SetNetID(int netId){

    // }
    public PlayerEntry[] GetPlayers(){
        PlayerEntry[] players = new PlayerEntry[Players.Count];
        for(int i = 0; i <Players.Count;i++){
            players[i]=Players[i];
        }
        return players;
    }
    public void LaunchRocket(GameObject rocketPrefab, Vector3 position, Quaternion rotation, float timer, float radius, int damagePoints, Vector3 direction){
        Debug.Log("launch rocket");
        if(isLocalPlayer){
        CmdLaunchRocket(rocketPrefab, position, rotation, timer, radius, damagePoints, direction);
        }
    }
    // misc
    [Command]
    public void CmdLaunchRocket(GameObject rocketPrefab, Vector3 position, Quaternion rotation, float timer, float radius, int damagePoints, Vector3 direction){
        Debug.Log("cmd launch rocket");
        GameObject rocketInstance = Instantiate(rocketPrefab,position, rotation);
        rocketInstance.GetComponent<MPRocketProjectile>().timer = timer;
        rocketInstance.GetComponent<MPRocketProjectile>().radius = radius;
        rocketInstance.GetComponent<MPRocketProjectile>().damagePoints = damagePoints;
        rocketInstance.GetComponent<MPRocketProjectile>().Launch(direction);
        NetworkServer.Spawn(rocketInstance);
        
    }
    
}
