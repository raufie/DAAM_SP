using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManagement : MonoBehaviour
{
    // THIS IS THE MAIN ENTRY POINT FOR INPUT CONTROl
    // Start is called before the first frame update
    
    // created
    public InputMaster controls;
    
    // assigned

    [SerializeField]
    public GameObject Player;
    public WeaponsInput weaponsInput;
    public MovementInput movementInput;
    public MountedGunInput mountedInput;

    void Awake(){
        
        controls = new InputMaster();
        // AGGREGATIONS 
       
        // new EnvironmentMapper();
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        
    }
    void Start(){
        movementInput = GetComponent<MovementInput>();
        weaponsInput = GetComponent<WeaponsInput>();
    }

   
    public InputMaster GetActions(){
        return controls;
    }

    public void loadControls(InputMaster _controls){
        controls = _controls;
        // weaponsInput.controls = controls;
        // movementInput.controls = controls;
        movementInput.SENSITIVITY = PlayerPrefs.GetFloat("sensitivity");
    }
    public void disable(){
        weaponsInput.disable();
        mountedInput.disable();
        movementInput.enabled = false;
        weaponsInput.enabled = false;
        Player.GetComponent<ThirdPersonController>().cursorDisabled = false;
        // should disable the scripts and show the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameObject.GetComponent<InputManagement>().enabled = false;

    }
    public void enable(){
     
        movementInput.enabled = true;
        weaponsInput.enabled = true;
        weaponsInput.enable();
        mountedInput.enable();
        
        
        Player.GetComponent<ThirdPersonController>().cursorDisabled = true;
        // should enable the scripts and hide the cursor
    }
        // void Move(V)
    private void OnEnable(){
        controls.Enable();
    }
    private void OnDisable(){
        controls.Disable();
    }
}
