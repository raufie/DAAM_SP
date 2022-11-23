using UnityEngine;
using UnityEngine.InputSystem;
public class MountedWeaponManager : MonoBehaviour
{

    public bool isMounted;
    public bool isInArea;
    public InputManagement InputManager;
    public GameObject PlayerPosition;
    public CameraManager cameraManager;
    public GameObject CamObject;
    public GameObject GunMount;
    
    private MountedGunInput mountedInput;
    private GameObject Player;



    [Header("LOOK CONTROLS")]
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    public float LeftClamp = -60.0f;
    public float RightClamp = 30.0f;
    public Vector2 _lookVector;

    private const float _threshold = 0.01f; 
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    public float SENSITIVITY;
    
    public enum WeaponType
    {
       MachineGun, GrenadeGun
    };
    public WeaponType weaponType;
    public WeaponBase[] weaponsList;
    
    public WeaponBase weapon;
    
    // inputs
    // public KeyMapperBasic keyMapper;
    public InputMaster controls;

    // objects
    public GameObject releaseObject;
    public GameObject rocketPrefab;
    public GameObject sparkMachine;
    public GameObject sparkGrenade;
    // INTERNAL
    private bool UsePressed;
    // Start is called before the first frame update
    void Awake(){
        RefreshInput();
        

    }
    void Start()
    {

        
        disMountGun();
        weaponsList = new WeaponBase[2];
        
        weaponsList[0] = new MachineGun("Machine gun",0.10f,releaseObject,1000,1000,1000,0,25,1);
        weaponsList[1] = new GrenadeGun("Grenade Gun",0.30f,releaseObject,50,50,50,0,55,0.25f, rocketPrefab, 5.0f, 5.0f);
        weapon = weaponsList[(int)weaponType];
        if(cameraManager == null){
            cameraManager = GameObject.FindGameObjectsWithTag("CONFIGURATION")[0].GetComponent<CameraManager>();
        }
    }

    void Update(){
        _lookVector = controls.Player.Look.ReadValue<Vector2>();

        // UsePressed = controls.Player.Use.ReadValue<float>()>=1f;
        if(UsePressed && !isMounted && isInArea ){
                Debug.Log("mounting now bitch");
                mountGun();
        }
        if(!UsePressed  && isMounted  ){
                Debug.Log("dismountingmounting now bitch");
                disMountGun();
        }
        bool isFirePressedDown = controls.Player.HoldShoot.ReadValue<float>() > 0.1f;
        if (isFirePressedDown && isMounted){
            
            if(weapon.IsFireable()){
                weapon.Fire();
                if(weaponType == WeaponType.MachineGun){
                    Instantiate(sparkMachine, releaseObject.transform.position, Quaternion.Euler(0,180f, 0));
                }else{
                    Instantiate(sparkGrenade, releaseObject.transform.position, Quaternion.Euler(0,180f, 0));

                }

            }
        }
        weapon.DrawRay();
        

    }
    private void LateUpdate(){
            if(isMounted){
            CameraRotation();
            UpdateGunMount();
            }
    }
    public void OnEnable(){
        // LOADING
        var rebinds = PlayerPrefs.GetString("rebinds");
        if(!string.IsNullOrEmpty(rebinds)){
            controls.LoadBindingOverridesFromJson(rebinds);
        }
        
        // controls.enabled = true;
    }
    void OnDisable(){
        // controls.disabled = true;
    }
    // Update is called once per frame
    public void mountGun(){
        cameraManager.SwitchCams(1);
        mountedInput.SetCurrent(gameObject);
        cameraManager.SetMountedGunCam(CamObject);

        // Use Input manager to disable other input
        InputManager.weaponsInput.disable();

        InputManager.movementInput.enabled = false;
        InputManager.weaponsInput.enabled = false;

      
        if(Player != null){
            SetPlayerTelemetry();
        }
        isMounted = true;
        // controls.Player.enabled = true;

    }
    public void disMountGun(){
        InputManager.enabled = true;
        InputManager.enable();
        isMounted = false;
        if(cameraManager != null){
        cameraManager.SwitchCams(0);
        }
        // controls.Player.enabled = false;
      
    }
    private void OnTriggerEnter(Collider other) {
        
        if (other.tag == "Player" && !isMounted){
            RefreshInput();
            RefreshAssignments();
            
           mountedInput.SetCurrent(gameObject);
           isInArea = true;
           Player = other.gameObject;
           cameraManager.SetMountedGunCam(CamObject);
           cameraManager.SwitchCams(0);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player" && !isMounted){
           isInArea = false;
           cameraManager.SwitchCams(0);
        }
        if (other.tag == "Player" && isMounted){
           isInArea = false;
           disMountGun();
        }
        
        
    }
    private void OnGUI() {
        if(isMounted){
        GUI.backgroundColor = Color.red;
        GUI.Button(new Rect(10+220, 20, 150, 80),weapon.getName()+ weapon.getCurrentMag()+ "/"+ weapon.getUnloadedAmmo() );
        GUI.Button(new Rect(10+220, 170, 150, 20),weapon.getStatus() );
        }
        if(isInArea && !isMounted){
        GUI.backgroundColor = Color.green;
        GUI.Button(new Rect(10+430, 20, 150, 80), "Mount Gun, PRESS E" );
        }
    }
    private void SetPlayerTelemetry(){
        // reset anims
        Animator PlayerAnimator = Player.GetComponent<Animator>();
        foreach(AnimatorControllerParameter parameter in PlayerAnimator.parameters) {            
                PlayerAnimator.SetBool(parameter.name, false);            
        }
        Player.transform.position = PlayerPosition.transform.position;
        Quaternion rotation = Quaternion.LookRotation(PlayerPosition.transform.forward);
        Player.transform.rotation = Quaternion.Euler(0.0f, rotation.eulerAngles.y, 0.0f);


    }
    
    private void CameraRotation(){
            // if there is an input and camera position is not fixed
            if (_lookVector.sqrMagnitude >= _threshold )
            {
                //Don't multiply mouse input by Time.deltaTime;
                // float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
                float deltaTimeMultiplier = 0.85f +SENSITIVITY;

                _cinemachineTargetYaw += _lookVector.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _lookVector.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            // _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            // _cinemachineTargetYaw = Mathf.Clamp(_cinemachineTargetYaw, LeftClamp, RightClamp);
            
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CamObject.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax){
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    private void UpdateGunMount(){
        Vector3 direction = weapon.GetMouseDirection();
        GunMount.transform.rotation = Quaternion.LookRotation(direction);
    }
    private void RefreshInput(){
        
        InputManager = GameObject.FindGameObjectsWithTag("InputManagement")[0].GetComponent<InputManagement>();
        mountedInput = InputManager.mountedInput;
        controls = InputManager.GetActions();
        if (controls == null){
            controls = new InputMaster();
        }
        controls.Enable();
    }
    private void RefreshAssignments(){
        controls.Player.Use.started -=Use;
        controls.Player.Use.started +=Use;
        // SET CAMERA OBJECT
        
    }
    private void Use(InputAction.CallbackContext ctx){
        if(isInArea){
             if (UsePressed){
             UsePressed = false;
             }else{
                UsePressed = true;
             }
            }
    }
}
