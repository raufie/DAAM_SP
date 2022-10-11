using UnityEngine;

public class TripodActions : ActionBase
{
    protected TripodController actions;
    private float CalibrationThreshold = 4f;
    public float CalibrationSlerpSpeed = 0.05f;
    void Awake()
    {
        actions = GetComponent<TripodController>();
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
        Debug.Log(player);
        actions.SlerpToTarget(player.transform.position, CalibrationSlerpSpeed);

        float angle = Vector3.Angle(actions.FirePoint.forward,player.transform.position - actions.FirePoint.position);
      
        if (angle < CalibrationThreshold){
            isCalibrated = true;
            return;
        }
        isCalibrated = false;
    }
       
    public override void Attack(){
        actions.SlerpToTarget(player.transform.position, CalibrationSlerpSpeed);

        actions.Fire();
        // actions.Target = player.GetComponent<Transform>();
        // actions.Launch();
    }
}
