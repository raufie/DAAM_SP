using UnityEngine;

public class Calibrating : State
{
    // Start is called before the first frame update
    public Calibrating(ObservationBase obs, ActionBase actions): base(obs, actions){
        name = STATE.CALIBRATING;
    }
    public override void Update(){
        // can go back to patrolling
        // can go forward to calibrating to attack 
        if(!obs.IsPlayerVisible() || !obs.IsPlayerInActivationRadius() || !obs.IsPlayerInAttackRadius()){
            base.nextState = new Patrolling(obs, actions);
            stage = EVENT.EXIT;
            // going back
        }else if(obs.IsPlayerVisible() && obs.IsPlayerInActivationRadius() && obs.IsPlayerInAttackRadius() && obs.IsCalibrated()){
            // going next

            base.nextState = new Attacking(obs, actions);
            stage = EVENT.EXIT;
        }
        
        else{
            actions.Calibrate();
            base.Update();
        }
        
    }
   
}
