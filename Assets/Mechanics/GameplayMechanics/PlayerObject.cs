using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    // Start is called before the first frame update
    public int Health;
    public GameObject DeadObject;
    [Header("ALL BELOW IS DEBUG")]
        
    public bool isDebug;
    public Material deadMaterial;
    private Rigidbody [] _childRbs;
    public GameObject BodyTarget;
    public GameObject HeadTarget;
    void Awake(){
        _childRbs = GetComponentsInChildren<Rigidbody>();
    }
    void Start(){
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
    private void OnGUI() {
        if(isDebug){
        GUI.backgroundColor = Color.yellow;
        GUI.Button(new Rect(20, 10, 150, 20)," Player Health: " + Health );
        }
    }
    private void EnableRagdoll(){
        foreach(var rg in _childRbs){
            rg.isKinematic = false;
        }
    }
    private void DisableRagdoll(){
        foreach(var rg in _childRbs){
            rg.isKinematic = true;
        }
    }
    private void KillPlayer(){
        Debug.Log("Player is dead");
        GameObject.FindGameObjectWithTag("InputManagement").GetComponent<InputManagement>().disable();
        GetComponent<Animator>().enabled = false;
        EnableRagdoll();
        DeadObject.SetActive(true);
        StartCoroutine(ResumeAfterFive());
    }
    IEnumerator ResumeAfterFive(){
        yield return new WaitForSeconds(5);
        StateManager.ResumeState();

    }
}
