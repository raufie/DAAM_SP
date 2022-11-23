using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class KillUpdateManager : MonoBehaviour
{
    public GameObject KillInfoPrefab;
    public RectTransform [] p; 
    public RectTransform StartingP;

    public RectTransform EndingP;
    public GameObject DyingObject;
    public GameObject []killObjects;

    

    public float LerpSpeed;
    private bool ToggleLerp;
    
    // p3 is top, p1 is the bottom and the newest ones go here
    // MANAGES kills bar where recent kills appear
    void Start(){
        killObjects = new GameObject[3];
        PushKill("userbro","secondone");
        
     

        // PushKill("userbro3","secondone3");

    }
    void FixedUpdate(){
        if(ToggleLerp){
            UpdateUI(true);
            // check for difference
            float diff = GetDifference();
            if(DyingObject!= null){
                // DyingObje
                DyingObject.GetComponent<CanvasGroup>().alpha = diff/10f;
            }
            
            if(diff <= 0.001f){
                ToggleLerp = false;
                Destroy(DyingObject);
                // PushKill("userbro2","secondone2");
            }
        }   
    }
    public void PushKill(string killer, string dead){    

        // Destroy(killObjects[2]);
        DyingObject = killObjects[2];
        killObjects[2] = killObjects[1];
        killObjects[1] = killObjects[0];
        killObjects[0] = null;
        GameObject Obj;
        
        Obj = Instantiate(KillInfoPrefab, StartingP.position, StartingP.rotation);
        Obj.transform.SetParent(transform);    
        Obj.GetComponent<KillInfo>().killer.text = killer;
        Obj.GetComponent<KillInfo>().dead.text = dead;
        killObjects[0] = Obj;
        ToggleLerp = true;
        return;       

    }
    public void UpdateUI(bool lerp = false){
        
        for(int i =0; i < killObjects.Length; i++){
            if(!lerp){
                if(killObjects[i]!= null){
                    killObjects[i].transform.position = p[i].position;
                }
            }else {
                if(killObjects[i]!= null){
                    killObjects[i].transform.position = Vector3.Lerp(killObjects[i].transform.position, p[i].position, LerpSpeed);
                }
            }
        }
        // FOR DYING OBJECTS
        if(!lerp){
                if(DyingObject!= null){
                    DyingObject.transform.position =EndingP.position;
                    Destroy(DyingObject);
                }
            }else {
                if(DyingObject!= null){
                    
                    DyingObject.transform.position = Vector3.Lerp(DyingObject.transform.position,EndingP.position, LerpSpeed);
                }
            }
        
    }
    public float GetDifference(){
        float differenceSum = 0f;
        for(int i = 0; i < killObjects.Length; i++){
            if(killObjects[i] != null){
                differenceSum+=Vector3.Distance(killObjects[i].transform.position,p[i].position);
            }
        }
        return differenceSum;
    }

    

}
