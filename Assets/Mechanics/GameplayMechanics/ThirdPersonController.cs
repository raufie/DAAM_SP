using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// REQUIRED COMPONENTS

// ANIMATOR
// WEAPONSMANAGER
public class ThirdPersonController : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

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
    // ASSIGNMENTS
    // JUMP and GRAVITY

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.4f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;
// GROUNDED
    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    private CharacterController _controller;
    private GameObject _mainCamera;
    private Animator _animator;
    private bool _hasAnimator;
    private WeaponsManager _weaponsManager;
    // ANIMATOR STATES
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int isCrouchingHash;
    int isAimingHash;
    int isShootingHash;
    int isReloadingHash;
// Plauer
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    // Start is called before the first frame update
    // INPUTS
    private InputMaster controls;
    public Vector2 _lookVector;
    // movement

    
    // CURSOR
    public bool cursorDisabled = true;
    void Awake(){
        controls = new InputMaster();

        if (_mainCamera == null)
        {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
    void Start()
    {
        // controls = GetComponent<InputManagement>().controls;   
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _hasAnimator = TryGetComponent(out _animator);
        _weaponsManager = GetComponent<WeaponsManager>();
        setUpAnimatorHashes();
        // _input = controls.Player;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(cursorDisabled){
        hideMouse();
        }
    }
    void FixedUpdate(){

        AddGravity();
        GroundedCheck();
        

    }

  

    public void Move(Vector2 moveVector, bool isSprinting, bool isCrouching) {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            // IF CROUCHING DIVIDE THE SPEED by some number
            // bool isCrouching = controls.Player.Crouch.ReadValue<float>() > 0.0f;
            float _SprintSpeed = SprintSpeed;
            float _MoveSpeed = MoveSpeed;
            if(isCrouching){
                _SprintSpeed *= 0.70f;
                _MoveSpeed *=0.70f;
            }
            // float targetSpeed = controls.Player.Sprint.ReadValue<float>() == 1.0f ? _SprintSpeed : MoveSpeed;
            float targetSpeed = isSprinting ? _SprintSpeed : MoveSpeed;

            
                // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            // Vector2 moveVector = controls.Player.Move.ReadValue<Vector2>();
            if (moveVector == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            // float inputMagnitude = _input.analogMovement ? moveVector.magnitude : 1f;
            float inputMagnitude =  moveVector.magnitude;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(moveVector.x, 0.0f, moveVector.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (moveVector != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

           
            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            //         // update animator if using character
            //         if (_hasAnimator)
            //         {
            //             _animator.SetFloat(_animIDSpeed, _animationBlend);
            //             _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            //         }
            // MOVE ANIMATORS
            // Debug.Log(moveVector);
            if(isCrouching){
                _animator.SetBool(isCrouchingHash, true);
            }else{
                _animator.SetBool(isCrouchingHash, false);
            }
            if(moveVector != Vector2.zero){
                _animator.SetBool(isWalkingHash, true);
                if (targetSpeed == _SprintSpeed ) {
                                _animator.SetBool(isRunningHash, true);
                }else{
                    _animator.SetBool(isRunningHash, false);

                }
            }else{
                _animator.SetBool(isWalkingHash, false);
                _animator.SetBool(isRunningHash, false);    

            }

    }
    private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            // Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            //     QueryTriggerInteraction.Ignore);
            
             Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
            
            
            // update animator if using character
            // if (_hasAnimator)
            // {
            //     _animator.SetBool(_animIDGrounded, Grounded);
            // }
    }
    private void AddGravity(){
        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
    }
    public void Jump(bool jumpInput , bool isAiming, bool isHoldingShoot)
        {
            if (Grounded && !isAiming && !isHoldingShoot)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(isJumpingHash, false);
                    // _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
                // Jump
                // Debug.Log(_jumpTimeoutDelta);
                if (jumpInput && _jumpTimeoutDelta <= 0.1f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(isJumpingHash, true);
                        // _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.1f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
                
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        // _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                jumpInput = false;
                //     // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
                // if (_verticalVelocity < _terminalVelocity)
                // {
                //     _verticalVelocity += Gravity * Time.deltaTime;
                // }
            }

            
        }
    
    public void Aim(bool isShooting, bool isAiming, bool isHolding, bool isSprinting){
        _animator.SetLayerWeight(1, 1);

        GameObject releaseObject = _weaponsManager.GetCurrentReleaseObject();
        if(isSprinting || !Grounded) return;

        if (isShooting || isAiming || isHolding)
        {
            Vector3 direction =_weaponsManager.GetCurrentWeapon().GetMouseDirection();
            Quaternion rotationRestricted = Quaternion.Euler(transform.rotation.eulerAngles.x,
                Quaternion.LookRotation(direction).eulerAngles.y, transform.rotation.eulerAngles.z);
            
            transform.rotation = rotationRestricted;
            if(_weaponsManager.GetCurrentWeapon().IsFireable() && (isShooting || isHolding)){
                _animator.SetBool(isShootingHash, true);

            }else{
                _animator.SetBool(isShootingHash, false);

            }
            _animator.SetBool(isAimingHash, true);

            // _animator.SetLayerWeight(1, 1);

            releaseObject.transform.parent.transform.rotation = Quaternion.LookRotation(direction);

        }else {
            _animator.SetBool(isAimingHash, false);
            // _animator.SetLayerWeight(1, 0);
            _animator.SetBool(isShootingHash, false);
        }
        // Reloading
        if (_weaponsManager.GetCurrentWeapon().isReloading ){
            _animator.SetBool(isReloadingHash, true);
            // _animator.SetLayerWeight(1, 1);
        }else{
            // _animator.SetLayerWeight(1, 0);
            _animator.SetBool(isReloadingHash, false);
        }

    }




// UPDATERS

    private void hideMouse(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void setUpAnimatorHashes(){
        isWalkingHash =Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isCrouchingHash = Animator.StringToHash("isCrouching");
        isAimingHash = Animator.StringToHash("isAiming");
        isShootingHash = Animator.StringToHash("isShooting");
        isReloadingHash = Animator.StringToHash("isReloading");
    }   
    // HELPERS
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax){
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    

}
