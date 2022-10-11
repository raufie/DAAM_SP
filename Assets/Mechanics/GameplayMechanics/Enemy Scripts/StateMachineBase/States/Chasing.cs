using UnityEngine;

public class Chasing : State
{
    // Start is called before the first frame update
    public Chasing(ObservationBase obs, ActionBase actions): base(obs, actions){
        name = STATE.CHASING;
    }
    public override void Update(){
        // can go back to patrolling
        // can go forward to calibrating to attack 
        if(!obs.IsPlayerVisible() || !obs.IsPlayerInActivationRadius()){
            base.nextState = new Patrolling(obs, actions);
            stage = EVENT.EXIT;
            // going back
        }else if(obs.IsPlayerVisible() && obs.IsPlayerInActivationRadius() && obs.IsPlayerInAttackRadius()){
            // going next

            base.nextState = new Calibrating(obs, actions);
            stage = EVENT.EXIT;
        }
        
        else{
        actions.Chase();
        base.Update();

        }
    }
   
}
