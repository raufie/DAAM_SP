using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkTelemetaryObject : NetworkBehaviour
{
    // Start is called before the first frame update
    [SyncVar]
    public int Health;
    public GameObject DeadObject;
    [Header("ALL BELOW IS DEBUG")]
        
    public bool isDebug;
    public Material deadMaterial;
    public Rigidbody [] _childRbs;
    public GameObject BodyTarget;
    public GameObject HeadTarget;
    void Awake(){
        _childRbs = GetComponentsInChildren<Rigidbody>();
    }
    void Start(){
        if(NetworkClient.localPlayer!= null){
            _childRbs = NetworkClient.localPlayer.gameObject.GetComponentsInChildren<Rigidbody>();
        }
        DisableRagdoll();
    }
    void Update(){
    }
    public void takeDamage(int damage)
    {
        if (Health <= 0)
        {
            // Destroy(gameObject);
            
            KillPlayer();
            
            
        }
        else if(Health - damage <0){
            Health = 0;
        }
        else{
            Health -= damage;
        }
        float color = ((((float)Health/100f))*255f);
        // GetComponent<Renderer>().material.color = new Color(255,color,243);
    }
    public void takeHealth(int healthToAdd){
        // add health but clamp to 100
        int newHealth = healthToAdd + Health;
        Health = Mathf.Clamp(newHealth, 0, 100);
    
        Debug.Log(Health);
    }
    public void OnGUI() {
        if(isDebug){
        GUI.backgroundColor = Color.yellow;
        GUI.Button(new Rect(20, 10, 150, 20)," Player Health: " + Health );
        }
    }
    public void EnableRagdoll(){
        GetComponent<Animator>().enabled = false;
        _childRbs = GetComponentsInChildren<Rigidbody>();
        foreach(var rg in _childRbs){
            rg.isKinematic = false;
        }
    }
    public void DisableRagdoll(){
        _childRbs = GetComponentsInChildren<Rigidbody>();
        foreach(var rg in _childRbs){
            rg.isKinematic = true;
        }
        GetComponent<Animator>().enabled = true;

    }
    public void KillPlayer(){
        Debug.Log("Player is dead");
        if(isLocalPlayer){
            DataManager.AddDeath();
        }
        // NetworkPlayerObject netObject = NetworkClient.localPlayer.gameObject.GetComponent<NetworkPlayerObject>();
        // netObject.GetComponent<Animator>().enabled = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<NetworkTelemetaryObject>().EnableRagdoll();
        // DeadObject.SetActive(true);
        GetComponent<NetworkTelemetaryObject>().StartCoroutine(ResumeAfterFive());
        
    }
    IEnumerator ResumeAfterFive(){
        yield return new WaitForSeconds(5);
        DisableRagdoll();
        NetworkPlayerObject netObject = NetworkClient.localPlayer.gameObject.GetComponent<NetworkPlayerObject>();
        // NetworkPlayerObject netObject = NetworkClient.localPlayer.gameObject.GetComponent<NetworkPlayerObject>();

        // NetworkClient.localPlayer.gameObject.transform.position = netObject.SpawnPoints[netObject.SpawnPoint].transform.position;

        gameObject.transform.position = netObject.SpawnPoints[netObject.SpawnPoint].transform.position;

    }
}
