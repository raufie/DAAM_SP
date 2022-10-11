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
    }
    void FixedUpdate(){
        // Patrol();
    
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
    
}
