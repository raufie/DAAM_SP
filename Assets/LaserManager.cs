using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public float Speed;
    public float timeout = 0.25f;
    public float launchTime;
    private bool isLaunched;
    private Vector3 Direction;
    void FixedUpdate(){
        if(Time.time > launchTime + timeout )
        {
            Destroy(gameObject);
        }
        // MOVE TOWARDS THE TARGET
        if(isLaunched){


        }
    }
    // Start is called before the first frame update
    public void Launch(Vector3 direction){
        
    }   
    public void LaunchConnected(Vector3 p1, Vector3 p2){
        launchTime = Time.time;
        GetComponent<LineRenderer>().SetPosition(0,p1);
        GetComponent<LineRenderer>().SetPosition(1,p2);
    }
}
