using UnityEngine;

public class DroneActions : ActionBase
{
    protected DroneController actions;
    public float CalibrationThreshold = 1f;
    public float CalibrationSlerpSpeed = 0.5f;
    void Awake()
    {
        base.Awake();

        actions = GetComponent<DroneController>();
        AudioType = "DroneStep";
        WalkingAudioTime = 2f;
    }
    public override void Patrol(){
      
        
        actions.SlerpToTarget(wayPointsArray[currentWayPoint].position, SlerpSpeed);
        
        if(Vector3.Distance(transform.position, wayPointsArray[currentWayPoint].position) < FinishDistance){
            NextWayPoint();
        }
        actions.MoveForward();
    }
    public override void Chase(){
        actions.SlerpToTarget(player.transform.position, SlerpSpeed);
        actions.MoveForward();
    }

    public override void Calibrate(){
        actions.SlerpToTarget(player.transform.position, CalibrationSlerpSpeed);

        float angle = Vector3.Angle(actions.FirePoint.transform.forward,player.transform.position - actions.FirePoint.transform.position);
        if (angle < CalibrationThreshold){
            isCalibrated = true;
            return;
        }
        isCalibrated = false;
    }
       
    public override void Attack(){
                
        Calibrate();
        actions.Fire();
        // actions.Target = player.GetComponent<Transform>();
        // actions.Launch();
    }
}
