using UnityEngine;

public class Patrolling : State
{
    // Start is called before the first frame update
    public Patrolling(ObservationBase obs, ActionBase actions): base(obs, actions){
        name = STATE.PATROLLING;
    }
    public override void Update(){
        if (obs.IsPlayerVisible() && obs.IsPlayerInActivationRadius()){
            // chase (the next state)
            base.nextState = new Chasing(obs, actions);
            stage = EVENT.EXIT;
        }else {
            actions.Patrol();
            base.Update();

        }
    }
   
}
