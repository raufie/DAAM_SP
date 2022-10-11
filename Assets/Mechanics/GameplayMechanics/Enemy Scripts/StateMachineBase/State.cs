using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    // Start is called before the first frame update
    public enum STATE {
        PATROLLING,
        CHASING,
        CALIBRATING,
        ATTACKING
    }
    public enum EVENT {
        ENTER, UPDATE, EXIT
    }
    
    protected ObservationBase obs;
    protected ActionBase actions; 

    public STATE name;
    protected EVENT stage;

    protected State nextState;

    public State(ObservationBase _obs, ActionBase _actions){
        stage = EVENT.ENTER;
        obs = _obs;
        actions = _actions;
    
    }

    public virtual void Enter() {stage = EVENT.UPDATE;}
    // here in the update we will use observation base and action base
    public virtual void Update() {
        // Debug.Log(name);
        stage = EVENT.UPDATE;
        }
    public virtual void Exit() {stage = EVENT.ENTER;}
    // params
   

    public State Process(){
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT) {
            Exit();
            return nextState;
        }
        return this;
    }
}
