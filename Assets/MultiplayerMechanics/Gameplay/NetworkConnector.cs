using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum ConnectionType {
    SERVER,
    CLIENT, 
    HOST
}
public class NetworkConnector : MonoBehaviour
{
    private MyNetworkManager manager;
    public ConnectionType Type;
    public Button LeaveGameBtn;
    public int MP_OPTIONS_SCENE;
    // public GameObject prefab3;
    void Awake(){
       
    }
    void Start(){
        AddListeners();
         manager = GetComponent<MyNetworkManager>();
        if(Type == ConnectionType.SERVER){
            manager.StartServer();
            //  manager.StartHost();
        }else if(Type == ConnectionType.HOST){
            manager.StartHost();
        }
        else{
            manager.StartClient();
            NetworkClient.Ready();
            // if (NetworkClient.localPlayer == null)
            // {
            //     NetworkClient.AddPlayer();
            // }
            // if u wanna add manually
        }
    }

    void LeaveGame(){
        manager.StopClient();
        SceneManager.LoadScene(MP_OPTIONS_SCENE);
    }
    void AddListeners(){

        LeaveGameBtn.onClick.AddListener(LeaveGame);
        
    }
}
