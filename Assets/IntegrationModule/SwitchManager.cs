using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class SwitchManager : MonoBehaviour
{
    // manages the operations taking place between the ui module
    // and the player module
    // Start is called before the first frame update
    public InputMaster controls;
    public GameObject UIObject;
    public InputManagement mechanicsInput;
    public Button ResumeBtn; 
    public Config config;
    public bool InGameOnStart = true;
    private bool isOpen = false;

    void Awake(){
        controls = new InputMaster();
        if(InGameOnStart){
                EnableInputs();
        }
    }
    void Start(){
        controls.Player.Escape.performed += (ctx)=>{
            Switch();        
        };
        ResumeBtn.onClick.AddListener(Switch);
        

    }
    public void Switch(){

        if(isOpen){
                EnableInputs();
            }else{
               DisableInputs();
            }
    }
    private void OnEnable(){
        controls.Enable();
    }
    private void OnDisable(){
        controls.Disable();
    }
    public void EnableInputs(){
            mechanicsInput.enabled = true;
            mechanicsInput.enable();
            UIObject.SetActive(false);
            config.UpdateConfiguration();
            Time.timeScale = 1f;
            isOpen = false;
    }
    public void DisableInputs(){
            mechanicsInput.disable();
            UIObject.SetActive(true);
            Time.timeScale = 0f;
            isOpen = true;
    }
    
}
