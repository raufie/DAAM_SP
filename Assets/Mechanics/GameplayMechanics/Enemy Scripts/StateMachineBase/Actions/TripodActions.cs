using UnityEngine;

public class TripodActions : ActionBase
{
    protected TripodController actions;
    public float CalibrationThreshold = 4f;
    public float CalibrationSlerpSpeed = 0.05f;
    public enum TripodType {
        BASIC,
        KAIJU,
        FINAL
    }
    public TripodType type = TripodType.BASIC;
    public float MeleeAttackRadius = 20f;
    void Awake()
    {
        actions = GetComponent<TripodController>();
        base.Awake();
        AudioType = "TripodStep";
        WalkingAudioTime = 1f;
    }
    void FixedUpdate(){
        base.FixedUpdate();
        if(GetComponent<StateMachine>().currentState.name == State.STATE.ATTACKING 
        ){
            WalkSound();
        }
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
        if(type == TripodType.KAIJU){
            actions.Fire(true);
        }else if(type == TripodType.BASIC){
            actions.Fire();
        }else if (type == TripodType.FINAL){
            // Debug.Log(Vector3.Distance(player.transform.position, transform.position));
            if(Vector3.Distance(player.transform.position, transform.position) < MeleeAttackRadius){
                actions.Melee();
            }else{
            actions.Fire(true);
            }
        }
        
        // actions.Target = player.GetComponent<Transform>();
        // actions.Launch();
    }
}
