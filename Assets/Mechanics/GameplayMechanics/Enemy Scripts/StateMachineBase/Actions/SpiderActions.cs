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
    }
    public override void Patrol(){
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
        actions.Launch();
    }
   
}
