using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ControlInterfaceTripod : MonoBehaviour
{
    public InputMaster controls;
    // public SpiderController controller;
    public TripodController controller;
    // Start is called before the first frame update
    void Awake () {
        controls = new InputMaster();
        controls.Player.Jump.performed += (ctx)=> Fire();
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
        
    }
    void Fire(){
        
        controller.Fire();
    }   
    private void OnEnable(){
        controls.Enable();
    }
    private void OnDisable(){
        controls.Disable();
    }
}
