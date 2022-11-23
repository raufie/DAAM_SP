using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MPConfig : MonoBehaviour
{
    public InputMaster controls;
    // Start is called before the first frame update
    public MPInputManagement mechanicsInput;
    public AudioManager audioManager;



    void Awake()
    {
        controls = new InputMaster();

    }

    void Start(){
        // LoadBindings();
        UpdateConfiguration();
    }

    public static void LoadBindings(InputMaster _controls){
        var rebinds = PlayerPrefs.GetString("rebinds");
        if(!string.IsNullOrEmpty(rebinds)){
            _controls.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public void UpdateConfiguration(){
        // update controls
        mechanicsInput.loadControls(controls);
        // update audio
        audioManager.load();
    }

    void OnEnable(){

    }
    void OnDisable(){

    }
}
