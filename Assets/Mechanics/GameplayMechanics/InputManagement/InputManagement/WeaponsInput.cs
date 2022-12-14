using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponsInput: MonoBehaviour
{

    public InputMaster controls;
    private WeaponsManager weaponsManager;

    public GameObject playerObject;

    void Awake(){
        playerObject = GameObject.FindGameObjectsWithTag("Player")[0];
        weaponsManager = playerObject.GetComponent<WeaponsManager>();
        controls = new InputMaster();

        Debug.Log("im updatiomg");

        controls.MachineGun.Disable();

      
        AddEvents();
               
    }
    void Start(){
        
    }
    void EmitFire(InputAction.CallbackContext ctx){
        
        if(weaponsManager.GetCurrentWeapon().IsFireable()){
            ThirdPersonController controller = playerObject.gameObject.GetComponent<ThirdPersonController>();
            // controller.Aim(true, true, true, false);
            weaponsManager.FireSemiWeapon();
        }

    }
     void Update(){
            bool isFirePressedDown = controls.Player.HoldShoot.ReadValue<float>() > 0.1f;
            if (isFirePressedDown){
                weaponsManager.FireAutoWeapon();
            }
    }
    
    void AddEvents(){
          // controls.Player.Shoot.performed  += (context)=> {
        //     weaponsManager.FireSemiWeapon();
        //     Debug.Log("weapon input fire");
        // };
        if(controls == null){
            controls = new InputMaster();
            Config.LoadBindings(controls);
        }
        controls.Player.Shoot.performed+=EmitFire;
        controls.Player.Reload.performed  += context=> weaponsManager.ReloadWeapon();

        controls.Player.WeaponSelection1.performed += context=> weaponsManager.SwitchWeapon(1);
        controls.Player.WeaponSelection2.performed += context=> weaponsManager.SwitchWeapon(2);
        controls.Player.WeaponSelection3.performed += context=> weaponsManager.SwitchWeapon(3);
        controls.Player.WeaponSelection4.performed += context=> weaponsManager.SwitchWeapon(4); 
    }
    void RemoveEvents(){
        controls.Player.Shoot.performed-=EmitFire;
    }
    // void Move(V)
    private void OnEnable(){
        Config.LoadBindings(controls);
        controls.Enable();
    }
    private void OnDisable(){
        RemoveEvents();
        controls.Disable();
    }
   
    public void disable(){
        RemoveEvents();
    }
    public void enable(){
        AddEvents();
    }
}
