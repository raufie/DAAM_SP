using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderActions : ActionBase
{
    protected SpiderController actions;


    void Awake()
    {
        actions = GetComponent<SpiderController>();
        base.Awake();
        // SET TYPE
        AudioType = "SpiderStep";
        WalkingAudioTime = 0.1f;
    }
    public override void Patrol(){
        // AUDIO CALIB



        // NORMAL CODE
        GetDesiredDirection();
        if (path.corners.Length > 1){
        actions.SlerpToTarget(path.corners[1], SlerpSpeed);
        }
        if(Vector3.Distance(transform.position, wayPointsArray[currentWayPoint].position) < FinishDistance){
            NextWayPoint();
        }
        actions.MoveForward();
    }
    public override void Chase(){

            
       
        GetDesiredDirection(player.transform.position);
        if (path.corners.Length > 1){
        actions.SlerpToTarget(path.corners[1], SlerpSpeed);
        }
        actions.MoveForward();
    }

    public override void Calibrate(){
        isCalibrated = true;
    }
    public override void Attack(){
        // actions.Target = player.GetComponent<Transform>();
  TimeFromLastStep = 0f;
        IsWalking = false;
        actions.Launch();
    }
   
}
