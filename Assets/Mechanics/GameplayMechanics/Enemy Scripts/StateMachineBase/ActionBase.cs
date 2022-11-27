using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ActionBase : MonoBehaviour
{
    public GameObject WayPoints;
    public float FinishDistance = 3f;
    public Transform [] wayPointsArray;
    public float SlerpSpeed = 0.1f;
    protected int currentWayPoint;
    protected NavMeshPath path;
    protected Vector3 nextPoint;
    public bool isCalibrated;
    public GameObject player;
    // Start is called before the first frame update
    // WALK SOUND ANIMS
    protected float TimeFromLastStep;
    protected bool IsWalking;
    public float WalkingAudioTime = 0.2f;
    protected string AudioType = "SpiderStep";
    // LASER PREFAB
    
    protected void Awake()
    {
        // SetAttributes();
        path = new NavMeshPath();
        // player = EnemyTargetManager.target;
        // if(player == null){
        //     player = GameObject.FindGameObjectsWithTag("Player")[0];
        // }
    }
    void Start(){
        // SetAttributes();
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerObject>().BodyTarget;
    }
    public void FixedUpdate(){
        // Patrol();
        if(GetComponent<StateMachine>().currentState.name == State.STATE.PATROLLING ||
        GetComponent<StateMachine>().currentState.name == State.STATE.CHASING ||
        GetComponent<StateMachine>().currentState.name == State.STATE.CALIBRATING
        ){
            WalkSound();
        }
    }
    
    public virtual void Patrol(){}
    public virtual void Chase(){}
    public virtual void Calibrate(){}
    public virtual void Attack(){}
    // INTERNAL
    protected void SetAttributes(){
        SetWayPointsArray();
        }
    protected void SetWayPointsArray(){
        wayPointsArray = WayPoints.GetComponentsInChildren<Transform>();
        Debug.Log(WayPoints.GetComponentsInChildren<Transform>().Length);
    }
    protected void NextWayPoint(){
        if(currentWayPoint < wayPointsArray.Length - 1){
            currentWayPoint+=1;
        }
        else if (currentWayPoint == wayPointsArray.Length - 1){
            currentWayPoint = 0;
        }
    }
    protected void GetDesiredDirection(Vector3 position = default(Vector3)){
        if(position == default(Vector3)){
        position = wayPointsArray[currentWayPoint].position;
        
            
        }
        NavMesh.CalculatePath(transform.position, position, NavMesh.AllAreas, path);
        for (int i = 0; i < path.corners.Length - 1; i++){
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }

    }
    protected void WalkSound(){
        try{
            AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
            // audioManager.fireSFXEvent("FootStep", playerObject.transform.position);
            if(IsWalking){
                Debug.Log("setting time");
                TimeFromLastStep = Time.time;
            }
            float TimeToStep = WalkingAudioTime;
            if(Time.time > TimeFromLastStep + TimeToStep ){
                audioManager.fireSFXEvent(AudioType, transform.position);
                TimeFromLastStep = Time.time;
            }
                      
        }catch{
            IsWalking = false;
            Debug.Log("ERROR INITIALIZING FOOTSTEL");
        }
    }
    
}
