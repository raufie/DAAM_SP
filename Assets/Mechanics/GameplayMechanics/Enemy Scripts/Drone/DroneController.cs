using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
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
    public bool isDebug = false;
    public GameObject debugSphere;
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
    // LASER
    public GameObject LaserPrefab;
    public GameObject Gun1;
    public GameObject Gun2;
    void Start()
    {

        _controller = GetComponent<CharacterController>();
        if (isDebug){
            SetupDebugging();
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
    public void RotateUp(){
        transform.Rotate(rotationSpeed,0f, 0f);
    }
    public void RotateDown(){
        transform.Rotate(-rotationSpeed,0f, 0f);
    }
    public void MoveUp(){
        _controller.Move(transform.up*speed/4);
    }
    public void MoveDown(){
        _controller.Move(transform.up*-1f*speed/4);
    }

    public void Fire(){
        if(Time.time > LastFired + FireRate && Time.time > LastBurstEnd + BurstWait){
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().fireSFXEvent("BlastarTie",transform.position);

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
        Vector3 p2;
        if(Physics.Raycast(FirePoint.position, transform.forward, out hit, Mathf.Infinity)){
            Debug.Log(hit.collider.tag);
            if(hit.collider.tag == "Player"){
                hit.collider.gameObject.GetComponent<PlayerObject>().takeDamage(DamagePoints);
                
            }
            p2 = hit.point;
            GameObject LaserObject = Instantiate(LaserPrefab, transform.position, Quaternion.identity);
            LaserObject.GetComponent<LaserManager>().LaunchConnected(Gun1.transform.position,p2);

            GameObject LaserObject2 = Instantiate(LaserPrefab, transform.position, Quaternion.identity);
            LaserObject2.GetComponent<LaserManager>().LaunchConnected(Gun2.transform.position,p2);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, GoalRotation, t);
    }
    private void SetupDebugging(){
        LaunchPosition = transform.position;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        debugSphere.SetActive(true);
        float radius = GetComponent<ObservationBase>().ActivationRadius;
        debugSphere.transform.localScale = new Vector3(radius, radius, radius);
        
    }

}
