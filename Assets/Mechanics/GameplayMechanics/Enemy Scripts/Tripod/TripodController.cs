using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripodController : MonoBehaviour
{
    private CharacterController _controller;
    public float speed;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public float VerticalVelocity ;
    public LayerMask GroundLayers;
    public float rotationSpeed;
    public Transform FirePoint;
    [Header("fire rate is no. of seconds it waits to fire on each shot")]

    public float FireRate;
    public float Bursts;
    public float BurstWait;
    [SerializeField]        
    public int DamagePoints;
// is debug?
    public bool isDebug =false;
    
// internal
    private float timeToTarget = 1.0f;   
    private bool Grounded = false;
    private float TimeLaunched;
    private Vector3 LaunchPosition;
    private bool isJumping;
    private int jumpFlag;//0:no jump, 1: jump initiated, 2:  outside gravity
    private Vector3 InitialVelocity;
    private LineRenderer lineRenderer;
    // FIRING INFORMATIon
    private float LastFired;
    private int CurrentBurst;
    private float LastBurstEnd;

    void Start()
    {

        _controller = GetComponent<CharacterController>();
        if (isDebug){
        }
       
    }
    void Update(){
        if(isDebug){
        DebugStuff();
        }
    }
    void FixedUpdate(){
        isGrounded();
           
        AddGravity();
        
        
            
    }
    public void MoveForward(){
        _controller.Move(transform.forward*speed/4);
    }
    public void MoveBackward(){
        _controller.Move(transform.forward*-1f*speed/4);
    }
    public void RotateLeft(){
        transform.Rotate(0f,-rotationSpeed, 0f);
    }
    public void RotateRight(){
        transform.Rotate(0f,rotationSpeed, 0f);
    }

    public void Fire(){
        if(Time.time > LastFired + FireRate && Time.time > LastBurstEnd + BurstWait){
            LastFired = Time.time;
            CurrentBurst+=1;
            HitRay();
            if (CurrentBurst == Bursts){
                LastBurstEnd = Time.time;
                CurrentBurst = 0;
            }
        }
        
    }

    public void HitRay(){
        RaycastHit hit;
        if(Physics.Raycast(FirePoint.position, transform.forward, out hit, Mathf.Infinity)){
            Debug.Log(hit.collider.tag);
            if(hit.collider.tag == "Player"){
                hit.collider.gameObject.GetComponent<PlayerObject>().takeDamage(DamagePoints);
            }
        }
    }

    private bool isGrounded(){
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);  
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
        QueryTriggerInteraction.Ignore);
        return Grounded;
    }
    private void AddGravity(){
        if(!Grounded){
            _controller.Move(Vector3.down*VerticalVelocity);
        }
    }
    void DebugStuff(){
        Debug.DrawRay(FirePoint.position, FirePoint.forward*100f, Color.red);
    }
    void OnGUI()
    {
        if(isDebug){
        GUI.color = Color.blue;
        GUI.Label(new Rect(10, 10, 100, 20), "Current Fired: "+CurrentBurst+ " / Bursts");
        GUI.Label(new Rect(30, 50, 200, 40), "Current Fired: "+CurrentBurst+ " / Bursts");
        }
    }
    public void SlerpToTarget(Vector3 target, float t){
        Vector3 lookPos = target - transform.position;
        
        Quaternion GoalRotation = Quaternion.LookRotation(lookPos);
        // transform.rotation = GoalRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,GoalRotation.eulerAngles.y,0),t);
    }

}
