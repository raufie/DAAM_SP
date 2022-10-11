using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // Start is called before the first frame update
    public State currentState;
    public ObservationBase obs;
    public ActionBase actions;
    void Start()
    {

        currentState = new Patrolling(obs, actions);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentState = currentState.Process();
        
    }
}
