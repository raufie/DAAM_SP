using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservationBase : MonoBehaviour
{
    GUIStyle  guiStyle = new GUIStyle();

    
    public float VisibilityDistance = Mathf.Infinity;
    public float ViewingAngle = 30f;
    public float ActivationRadius = 15f;
    public float AttackRadius = 5f;

    public GameObject target;
    private ActionBase actions;
    public GameObject Eye;
    public bool isDebug = false;
    void Start(){
        
        actions = GetComponent<ActionBase>();
        // get target
        target =  GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerObject>().BodyTarget;


    }
    public bool IsPlayerVisible(){
        // combination of (is player blocked and is it in the viewing angle)
        if (!IsBlocked() && IsInView()){
            return true;
        }
        return false;
    }
    public bool IsPlayerInActivationRadius(){
        if (Vector3.Distance(target.transform.position, transform.position) < ActivationRadius){
            return true;
        }
        return false;
    } 
    public bool IsPlayerInAttackRadius(){
        if (Vector3.Distance(target.transform.position, transform.position) < AttackRadius){
            return true;
        }
        return false;
    }
    public bool IsCalibrated(){
        // check whether the actions are calibrated
        
        return actions.isCalibrated;  
    }
    // UTILITIES
    private bool IsBlocked(){
        RaycastHit hit;
        Transform EyeTransform = transform;
        if(Eye != null){
            EyeTransform = Eye.transform;
        }
        Debug.DrawRay(EyeTransform.position, target.transform.position - EyeTransform.position, Color.yellow, Time.deltaTime );
        if(Physics.Raycast(EyeTransform.position, target.transform.position - EyeTransform.position,  out hit,VisibilityDistance)){
            if(hit.collider.tag == "Player"){
            return false;    
            }            
        }
        return true;
    }
    private bool IsInView(){
        Transform EyeTransform = transform;
        if(Eye != null){
            EyeTransform = Eye.transform;
        }
        float angle = Vector3.Angle(EyeTransform.forward,target.transform.position - EyeTransform.position);
        if (angle < ViewingAngle){
            return true;
        }
        return false;
    }
    private void OnGUI ()
     {  
        if(isDebug){
            guiStyle.fontSize = 50; //change the font size
            if(IsPlayerVisible()){
            GUILayout.Label("PLAYER VISIBLE", guiStyle);
            //  GUI.Label(GUI.Rect(650, 650, 300, 50), "VISIBLE IS PLAYER", style);

            }
        }

     }

}
