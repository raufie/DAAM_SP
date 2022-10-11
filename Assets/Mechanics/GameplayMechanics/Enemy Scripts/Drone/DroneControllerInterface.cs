using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class DroneControllerInterface : MonoBehaviour
{
    public InputMaster controls;
    // public SpiderController controller;
    public DroneController controller;
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
            // rotate up
            controller.RotateUp();
        }
        if(controls.Player.Use.ReadValue<float>()== 1.0f){
            // rotate down
            controller.RotateDown();
           

        }
        if(controls.Player.Crouch.ReadValue<float>()== 1.0f){
            // alt+
            controller.MoveUp();
        }
        if(controls.Player.Sprint.ReadValue<float>()== 1.0f){
            // alt-
            controller.MoveDown();
        }
        Debug.Log(controls.Player.Reload.ReadValue<float>());
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
