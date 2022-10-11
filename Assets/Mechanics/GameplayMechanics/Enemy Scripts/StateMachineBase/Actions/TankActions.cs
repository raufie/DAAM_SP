using UnityEngine;

public class TankActions : ActionBase
{
    protected TankController actions;
    public float CalibrationThreshold = 1f;
    public float CalibrationSlerpSpeed = 0.5f;
    void Awake()
    {
        actions = GetComponent<TankController>();
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
        actions.SlerpTurret(player.transform.position, CalibrationSlerpSpeed);

        float angle = Vector3.Angle(actions.FirePoint.transform.forward,player.transform.position - actions.FirePoint.transform.position);
        Debug.Log(angle);
        if (angle < CalibrationThreshold){
            isCalibrated = true;
            return;
        }
        isCalibrated = false;
    }
       
    public override void Attack(){
                
        actions.SlerpTurret(player.transform.position, CalibrationSlerpSpeed);

        actions.Fire();
        // actions.Target = player.GetComponent<Transform>();
        // actions.Launch();
    }
}
