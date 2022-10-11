using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    public enum STATE {
        ALIVE, INACTIVE
    }
    public STATE state = STATE.INACTIVE;
    public void SetAlive(){
        state = STATE.ALIVE;
    }
    public void SetInactive(){
        state = STATE.INACTIVE;
    }
    public string GetStateString(){
        if(state == STATE.ALIVE){
            return "ALIVE";
        }else{
            return "INACTIVE";
        }
    }
    public bool IsAlive(){
        return state == STATE.ALIVE;
    }
}
