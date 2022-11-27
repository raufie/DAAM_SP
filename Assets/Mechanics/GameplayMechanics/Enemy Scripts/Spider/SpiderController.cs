using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
// SETUP
    private CharacterController _controller;
    public float speed;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public float VerticalVelocity ;
    public LayerMask GroundLayers;
    public float rotationSpeed;
    public float LaunchGravity = 1f;
    public Transform Target;
    public Vector3 LaunchVelocityRestriction;
    public float Restriction=10f;
    [SerializeField]
    public float LaunchRadius;
    public float velocity;
    public float FireDelay = 1f;
// is debug?
    public bool isDebug =false;
    public GameObject debugSphere;
    
    public int DamagePoints = 15;
    public float GROUND_Y = -1.5f;
    public float SAFE_Y = 1.5f;

// internal
    private float timeToTarget = 1.0f;   
    private bool Grounded = false;
    private float TimeLaunched;
    private Vector3 LaunchPosition;
    private bool isJumping;
    private int jumpFlag;//0:no jump, 1: jump initiated, 2:  outside gravity
    private Vector3 InitialVelocity;
    private LineRenderer lineRenderer;
    private bool isTouchingSomething;
    // timing fire
    private float lastFired;
    private int payload = 1;
    // AUDIO
    
    // Start is called before the first frame update
    void Start()
    {
        Target =  GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerObject>().HeadTarget.transform;
        CalibrateVelocity(timeToTarget);

        _controller = GetComponent<CharacterController>();
        if (isDebug){
        SetupDebugging();
        }
       
    }
    void FixedUpdate(){
           isGrounded();
           
           if(isJumping){
            if(IsDone()){   
                isJumping = false;
            }
                ApplyJump();
            
            if (Grounded == false && jumpFlag == 1){
                jumpFlag = 2;
            }
            if (jumpFlag == 2 && Grounded == true){
                jumpFlag = 0;
                isJumping = false;
            }
           }else{
            AddGravity();
            if(isDebug){
            DebugVelocity();
            }
           }
           if (transform.position.y < GROUND_Y){
                transform.position = new Vector3(transform.position.x, SAFE_Y, transform.position.z);
           }
       
            
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
    public void Launch(){
        if (!isFirable()){
            return;
        }
        if(Vector3.Distance(Target.position, transform.position) > LaunchRadius)
            return;
        lastFired = Time.time;
        LaunchPosition = transform.position;
        payload = 1;
        // calculate time to target based on velocity t = s/v

        timeToTarget = Vector3.Distance(LaunchPosition, Target.position) / velocity;
        CalibrateVelocity(timeToTarget, true);
        jumpFlag = 1;
        isJumping = true;
        TimeLaunched = Time.time;
        
    }
    private Vector3 CalibrateVelocity(float t_hit, bool restrict = false){
        // calculate vi for x,y,z
        float Sx = Target.position.x - transform.position.x ;
        float Sy =Target.position.y - transform.position.y;
        float Sz = Target.position.z - transform.position.z;
        float vx = (Sx)/t_hit;
        float vy = (Sy +0.5f*LaunchGravity*(t_hit*t_hit))/t_hit;

        float vz = (Sz)/t_hit;
        // float vy = LaunchGravity * t_hit;
        InitialVelocity = new Vector3(vx,vy,vz);
        // return (Target.position - transform.position).normalized;
        if (restrict){
            InitialVelocity = GetRestrictedVelocity(InitialVelocity);
        }
        return InitialVelocity;
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
    private void ApplyJump(){
        // update the transform in a parabolic manner
        float t = Time.time - TimeLaunched;
        
        // Vector3 V = new Vector3(0f, 10f, 10f);
        // Vector3 V  = 
        Vector3 S = GetNextPositionFromVelocity(InitialVelocity, t);
        // Debug.Log(Sz);
        // transform.position = new Vector3(transform.position.x+S.x,LaunchPosition.y+S.y,  LaunchPosition.z + S.z);

        transform.position = S;
    }
    private Vector3 GetNextPositionFromVelocity(Vector3 V, float t ){
        
        float Sx = V.x*t;
        float Sy = -0.5f*LaunchGravity*(Mathf.Pow(t,2)) + V.y * t;

        float Sz =  V.z * t;
        return new Vector3(LaunchPosition.x+Sx,LaunchPosition.y+Sy,LaunchPosition.z+Sz);
    }
    private void SetupDebugging(){
        LaunchPosition = transform.position;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        debugSphere.SetActive(true);
        debugSphere.transform.localScale = new Vector3(LaunchRadius, LaunchRadius, LaunchRadius);
        
    }
    private void DebugVelocity(){
        timeToTarget = Vector3.Distance(LaunchPosition, Target.position) / velocity;

        CalibrateVelocity(timeToTarget);
      
        LaunchPosition = transform.position;
        
        float [] times= new float[8] {
            0.1f,0.5f,1.0f,1.5f, 2.0f,2.5f,3.0f,3.5f
        };
        
        Vector3 V = InitialVelocity;
        // Debug.Log(V);
        lineRenderer.SetVertexCount(times.Length);
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        Color c1 = new Color(1f, 0.92f, 0.016f,0.55f);
        Color c2 = new Color(1f, 0.92f, 0.016f,0.55f);

       
        for (int i = 0; i < times.Length; i++){
            lineRenderer.SetPosition(i, GetNextPositionFromVelocity(V, times[i]));
        }
         lineRenderer.SetColors(c1,c2);

    }
    private Vector3 GetRestrictedVelocity(Vector3 vel){
        // float x = Mathf.Clamp(vel.x, -LaunchVelocityRestriction.x,LaunchVelocityRestriction.x);
        // float y = Mathf.Clamp(vel.y, -LaunchVelocityRestriction.y,LaunchVelocityRestriction.y);
        // float z = Mathf.Clamp(vel.z, -LaunchVelocityRestriction.z,LaunchVelocityRestriction.z);
        if(vel.x >  Restriction|| vel.y >Restriction || vel.z > Restriction){
            return vel.normalized * Restriction;    
        }
        return vel;
    }
    public void SlerpToTarget(Vector3 target, float t){
        Vector3 lookPos = target - transform.position;
        
        Quaternion GoalRotation = Quaternion.LookRotation(lookPos);
        // transform.rotation = GoalRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,GoalRotation.eulerAngles.y,0),t);
    }
    private bool isFirable(){
        if (Time.time > lastFired + FireDelay){
            return true;
        }
        return false;
    }
    private void OnTriggerEnter(Collider other) {
        isTouchingSomething = true;
        if (other.tag == "Player"){
            GiveDamageToPlayer(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other) {
        isTouchingSomething = false;
    }
    private bool IsDone(){
        bool touched = Time.time - lastFired > 0.3f && isTouchingSomething ;
         return touched || Vector3.Distance(LaunchPosition , transform.position) > Restriction;
    }
    public void GiveDamageToPlayer(GameObject player){
        payload = 0;
        Debug.Log("GIUVING DAMAGE TO PLAYER");

        player.GetComponent<PlayerObject>().takeDamage(DamagePoints);
    }
}
