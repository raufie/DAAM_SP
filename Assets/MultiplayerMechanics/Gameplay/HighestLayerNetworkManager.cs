using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HighestLayerNetworkManager : MyNetworkManager
{
    // public MyNetworkManager manager;
    public bool IsServer;
    public Button LeaveGameBtn;
    public int MP_OPTIONS_SCENE;
    public override void Awake(){
        base.Awake();
        if(IsServer){
            StartServer();
        }else{
            StartClient();
        }
    }
    public override void Start(){
        base.Start();
        AddListeners();

    }

    void LeaveGame(){
        StopClient();
        SceneManager.LoadScene(MP_OPTIONS_SCENE);
    }
    void AddListeners(){

        LeaveGameBtn.onClick.AddListener(LeaveGame);
        
    }
}
