using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class TankControllerInterface : MonoBehaviour
{
    public InputMaster controls;
    // public SpiderController controller;
    public TankController controller;
    // Start is called before the first frame update
    void Awake () {
        controls = new InputMaster();
        controls.Player.Shoot.performed += Fire;
        // Reload, Use, Crouch, Sprint
    }
    void Update(){
        WASD();
    }
    void WASD(){
        Vector2 moveVector = controls.Player.Move.ReadValue<Vector2>();
        if(moveVector[1] == 1f){
            controller.MoveForward();
        }
        if (moveVector[1] == -1f){
            controller.MoveBackward();
        }
        if (moveVector[0] == 1f){
            controller.RotateRight();
        }
        if (moveVector[0] == -1f){
            controller.RotateLeft();
        }
        if (controls.Player.Reload.ReadValue<float>()== 1.0f){
            // rotate turrel right
            controller.RotateTurretRight();
        }
        if(controls.Player.Use.ReadValue<float>()== 1.0f){
            // rotate turret left
            controller.RotateTurretLeft();

        }
        if(controls.Player.Crouch.ReadValue<float>()== 1.0f){
            // rotate barrel down
        }
        if(controls.Player.Crouch.ReadValue<float>()== 1.0f){
            // rotate barrel up
        }
    }
    void Fire(InputAction.CallbackContext ctx){
        
        controller.Fire();
    }   
    private void OnEnable(){
        controls.Enable();
    }
    private void OnDisable(){
        controls.Disable();
    }
}
