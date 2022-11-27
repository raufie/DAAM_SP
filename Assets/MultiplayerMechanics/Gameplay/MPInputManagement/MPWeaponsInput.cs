using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
public class MPWeaponsInput: MonoBehaviour
{

    public InputMaster controls;
    private MPWeaponsManager weaponsManager;
    // private MPConfig Config;
    public GameObject playerObject;

    void Awake(){
        controls = new InputMaster();
    }

    void Start(){
        if(NetworkClient.localPlayer != null){
            playerObject = NetworkClient.localPlayer.gameObject;
            weaponsManager = playerObject.GetComponent<MPWeaponsManager>();
            controls = new InputMaster();

            Debug.Log("im updatiomg");

            // controls.MachineGun.Disable();

        
            AddEvents();
        }
    }
    void Update(){
            
            bool isFirePressedDown = controls.Player.HoldShoot.ReadValue<float>() == 1.0f;
            
            if (isFirePressedDown){
                weaponsManager.FireAutoWeaponMP();
            }
    }
    void EmitFire(InputAction.CallbackContext ctx){
        
        if(playerObject == null){
            RemoveEvents();
            return;
        }
        if(weaponsManager.GetCurrentWeapon().IsFireable()){
            MPThirdPersonController controller = playerObject.gameObject.GetComponent<MPThirdPersonController>();
            // controller.Aim(true, true, true, false);
           GameObject hitPlayer = weaponsManager.FireSemiWeaponMP();
           if(hitPlayer != null){
                // TARGET RPC
                playerObject.GetComponent<NetworkPlayerObject>().CmdDamage(hitPlayer, 25);
                Debug.Log(hitPlayer.GetComponent<NetworkTelemetaryObject>().Health);
                if(hitPlayer.GetComponent<NetworkTelemetaryObject>().Health == 0){

                    hitPlayer.GetComponent<NetworkTelemetaryObject>().KillPlayer();
                    playerObject.GetComponent<NetworkPlayerObject>().CmdAddKill(playerObject.GetComponent<NetworkPlayerObject>().Team, playerObject.GetComponent<NetworkPlayerObject>().ID, playerObject, hitPlayer);
                    DataManager.AddKill();
                }
           }
        }

    }
  
    
    void AddEvents(){
          // controls.Player.Shoot.performed  += (context)=> {
        //     weaponsManager.FireSemiWeapon();
        //     Debug.Log("weapon input fire");
        // };
        if(controls == null){
            controls = new InputMaster();
            MPConfig.LoadBindings(controls);
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
        if(controls == null){
            controls = new InputMaster();
        }
        AddEvents();
        MPConfig.LoadBindings(controls);
        controls.Enable();
        
    }
    private void OnDisable(){
        RemoveEvents();
        controls.Disable();
    }
   
    public void disable(){
        RemoveEvents();
        controls.Disable();
    }
    public void enable(){
        AddEvents();
    }
}
