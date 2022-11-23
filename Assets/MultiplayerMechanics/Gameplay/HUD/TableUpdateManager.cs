using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TableUpdateManager : MonoBehaviour
{
    public GameObject TableEntryPrefab;
    public RectTransform [] p; 
    public GameObject [] EntryObjects;

    

    public float LerpSpeed;
    private bool ToggleLerp;
    
    // p3 is top, p1 is the bottom and the newest ones go here
    // MANAGES kills bar where recent kills appear
    void Start(){
        EntryObjects = new GameObject[6];
        // PushEntry("userbro","secondone");
        // AddEntryAtIndex("user123", "34", "23", 1);
        // AddEntryAtIndex("use35323", "34", "23", 0);
        // AddEntryAtIndex("user999", "4", "0", 2);


        // PushEntry("userbro3","secondone3");

    }
    void FixedUpdate(){

    }
    public void AddEntryAtIndex(string username, string kills, string deaths, int index){    

        
        GameObject Obj;
        
        Obj = Instantiate(TableEntryPrefab, p[index].position, p[index].rotation);
        Obj.transform.SetParent(transform);    
        Obj.GetComponent<PlayerTableEntry>().username.text = username;
        Obj.GetComponent<PlayerTableEntry>().kills.text = kills;
        Obj.GetComponent<PlayerTableEntry>().deaths.text = deaths;
        if(EntryObjects.Length == 0){
            EntryObjects = new GameObject[6];
        }
        
        EntryObjects[index] = Obj;
        UpdateUI();
        return;       

    }
    public void UpdateUI(){
        
        for(int i =0; i < EntryObjects.Length; i++){
            
                if(EntryObjects[i]!= null){
                    EntryObjects[i].transform.position = p[i].position;
                }
            
        }

    }

    public void UpdateInformation(){
        // get information..
        // update values
        // call update ui
    }

    

}
