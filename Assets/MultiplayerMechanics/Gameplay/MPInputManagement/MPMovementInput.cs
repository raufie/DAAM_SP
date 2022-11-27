using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPMovementInput : MonoBehaviour
{
    public InputMaster controls;
    private MPThirdPersonController controller;

    public GameObject playerObject;
    public Vector2 _lookVector;
    // Start is called before the first frame update

    // CAMERAS
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    // LOOK CAMERA ATTRS
    public bool LockCameraPosition = false;
    private const float _threshold = 0.01f;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    public float SENSITIVITY;
    // SPRINT CAPPING
    private float LastSprintTime;
    public float SprintLimit;
    public bool Enabled = true;
    // CAMS
    void Awake(){
        // playerObject = GetComponent<InputManagement>().Player;
        // controller = playerObject.GetComponent<ThirdPersonController>();
        controls = new InputMaster();
        // CinemachineCameraTarget = controller.CinemachineCameraTarget;
    }

    void Update(){
        
        // /WASD
        bool isCrouching = controls.Player.Crouch.ReadValue<float>() > 0.0f;
        bool isSprinting = controls.Player.Sprint.ReadValue<float>() == 1.0f ;
        Vector2 moveVector = controls.Player.Move.ReadValue<Vector2>();
        // JUMP
        bool jumpInput = controls.Player.Jump.triggered;
        bool isAiming = controls.Player.Aim.ReadValue<float>() == 1.0;
        bool isHoldingShoot = controls.Player.HoldShoot.ReadValue<float>() == 1.0;
        if(isHoldingShoot == true){
            isAiming = true;
        }
        if(Enabled){
        controller.Move(moveVector, isSprinting, isCrouching, isAiming);

        controller.Jump(jumpInput, isAiming, isHoldingShoot);
        // UPDATE LOOK VECTOR
       _lookVector = controls.Player.Look.ReadValue<Vector2>();
        }


    }
    void FixedUpdate(){

        bool isSprinting = controls.Player.Sprint.ReadValue<float>() == 1.0f ;
        bool isShooting = controls.Player.Shoot.triggered ;
        bool isAiming = controls.Player.Aim.ReadValue<float>() >0.0;
        bool isHolding = controls.Player.HoldShoot.ReadValue<float>()>0.0 ;

        if(isHolding == true){
            isAiming = true;
        }
        if(Enabled){
            controller.Aim(isShooting, isAiming, isHolding, isSprinting);
        }
    }

    private void LateUpdate(){

            CameraRotation();
    }
    private void CameraRotation(){
            // if there is an input and camera position is not fixed
            if (_lookVector.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                // float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
                float deltaTimeMultiplier = 0.85f +SENSITIVITY;

                _cinemachineTargetYaw += _lookVector.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _lookVector.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax){
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    private void OnEnable(){
        controls.Enable();
        playerObject = GetComponent<MPInputManagement>().Player;
        controller = playerObject.GetComponent<MPThirdPersonController>();
        CinemachineCameraTarget = controller.CinemachineCameraTarget;
    }
    private void OnDisable(){
        controls.Disable();
    }
   
    public void disable(){

    }
    public void enable(){

    }
}
