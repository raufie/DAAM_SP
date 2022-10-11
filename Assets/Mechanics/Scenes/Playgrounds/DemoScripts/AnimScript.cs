using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript : MonoBehaviour
{
    public Animator animator;
    public InputMaster controls;
    int isWalkingHash;
    int isRunningHash;
    void Awake(){
        controls = new InputMaster();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }
    void FixedUpdate(){
        // vert:[1]: up: 1, down: -1, 0: no movement, 
        // horiz:[0]: right: 1, left: -1, 0: no movement,
        int movementHash;
        Vector2 movement = controls.Player.Move.ReadValue<Vector2>();
        float sprint = controls.Player.Sprint.ReadValue<float>();
        if(sprint > 0.1f && movement[1]==1.0f){
            movementHash = isRunningHash;
        }else{
            movementHash = isWalkingHash;
        }
        if ((int)movement[0] == 1){
            Debug.Log("going right");
        }
        if((int)movement[0] == -1){
            Debug.Log("going left");
        }
        if((int)movement[1] == 1){
        
           animator.SetBool(movementHash, true);
        }
        if((int)movement[1] == 0){
            animator.SetBool(movementHash, false);
        }
        if((int)movement[1] == -1){
            Debug.Log("going down");
        }
        
    }
    void OnEnable(){
        controls.Enable();
    }
    void OnDisable(){
        controls.Disable();
    }


    
}
