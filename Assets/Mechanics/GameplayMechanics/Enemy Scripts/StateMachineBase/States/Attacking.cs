using UnityEngine;

public class Attacking : State
{
    // Start is called before the first frame update
    public Attacking(ObservationBase obs, ActionBase actions): base(obs, actions){
        name = STATE.ATTACKING;
    }
    public override void Enter(){
        actions.Attack();
        base.Enter();
    }
    public override void Update(){
        // can go back to patrolling
        // can go forward to calibrating to attack 
        if(!obs.IsPlayerVisible() || !obs.IsPlayerInActivationRadius() || !obs.IsPlayerInAttackRadius() || !obs.IsCalibrated()){
            base.nextState = new Patrolling(obs, actions);
            stage = EVENT.EXIT;
            // going back
        }        
        else {
        actions.Attack();
        base.Update();

        }
    }
   
}
