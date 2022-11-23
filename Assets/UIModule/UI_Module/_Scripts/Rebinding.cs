using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
public class Rebinding : MonoBehaviour
{
    [Header("Slider for sensitivity")]
    public Slider mouseSlider;
    [Header("Use the order of these Input actions to assign buttons and texts")]
    public InputAction WeaponSelection1;
    public InputAction WeaponSelection2;
    public InputAction WeaponSelection3;
    public InputAction WeaponSelection4;
    public InputAction Use;
    public InputAction Shoot;
    public InputAction Jump;
    public InputAction Crouch;
    public InputAction Sprint;    
    public InputAction Info;
    public InputAction Reload;
    

    [Header("Movement Input is separate")]
    InputAction Move;
    public InputAction MoveUp;
    public InputAction MoveDown;
    public InputAction MoveLeft;
    public InputAction MoveRight;
    
    


// THIS ARRAY IS HELPER... jsut puts all these actions in an array
    private InputAction [] actionVars;
    private InputMaster controls;

    [Header("Assign the correct button for the weapon selection based on the order of input actions")]
    public Button [] buttons;
    [Header("Assign text in the order of the input actions")]
    [Header("Last 4 text assignments are for UP, Down, Left, Right")]
    public TMP_Text [] textValues;

    [Header("Button Choice OVERLAY")]
    [SerializeField]
    public GameObject overlay;

    [Header("Movement Buttons")]
    public Button [] movementButtons;
    [Header("Movement Text")]
    public TMP_Text [] movementText;

    public InputAction Look;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    
    void Awake(){
        controls = new InputMaster();
        // THIS ASSIGNMENT IS VIP AND STATIC
        WeaponSelection1 = controls.Player.WeaponSelection1;
        WeaponSelection2 = controls.Player.WeaponSelection2;
        WeaponSelection3 = controls.Player.WeaponSelection3;
        WeaponSelection4 = controls.Player.WeaponSelection4;
// Movement
        Move = controls.Player.Move;
        Look = controls.Player.Look;
        Use = controls.Player.Use;

        Shoot = controls.Player.Shoot;
        Jump = controls.Player.Jump;
        Crouch = controls.Player.Crouch;
        Sprint = controls.Player.Sprint;    
        Info = controls.Player.Info;
        Reload = controls.Player.Reload;
        actionVars = new InputAction[] {
            WeaponSelection1,
            WeaponSelection2, 
            WeaponSelection3, 
            WeaponSelection4,
            Use,
            Shoot,
            Jump,
            Crouch,
            Info,
            Reload
            };

    }
    // Start is called before the first frame update
    void Start()
    {
        SetUpListeners();
        UpdateText();
        // int bindingIndexJump = controls.Player.kick.GetBindingIndexForControl(controls.Player.kick.controls[0]);
        // int bindingIndexCrouch = controls.Player.crouch.GetBindingIndexForControl(controls.Player.crouch.controls[0]);

        // WeaponSelection1Btn.onClick.AddListener(()=>StartRebinding(controls.Player.WeaponSelection1));
        // WeaponSelection2Btn.onClick.AddListener(()=>StartRebinding(controls.Player.WeaponSelection2));
        // text.text = InputControlPath.ToHumanReadableString(
        //     controls.Player.kick.bindings[bindingIndexJump].effectivePath,
        //     InputControlPath.HumanReadableStringOptions.OmitDevice
        // );
        // text2.text = InputControlPath.ToHumanReadableString(
        //     controls.Player.crouch.bindings[bindingIndexCrouch].effectivePath,
        //     InputControlPath.HumanReadableStringOptions.OmitDevice
        // );
    }   

    public void StartInteractiveRebind(InputAction action, int index){
        int bindingIndex = action.GetBindingIndexForControl(action.controls[0]);
        if(action.bindings[bindingIndex].isComposite){
            var firstPartIndex = bindingIndex+1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite){
                StartRebinding(action, index);

            }else{
                StartRebinding(action, index);
            }
        }
    }

    void StartRebinding(InputAction action, int index, bool isMovement=false, int movementBindingIndex=-1){
        overlay.SetActive(true);
        // we can specify what we dont want
        if(isMovement){
            rebindingOperation = action.PerformInteractiveRebinding(movementBindingIndex)
            .WithTargetBinding(movementBindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete((operation)=>{
                // check if it is a duplicate
                if(CheckDuplicateBindings(action, index, true, movementBindingIndex)){
                    // CLEAN UP
                    rebindingOperation.Dispose();

                    action.RemoveBindingOverride(movementBindingIndex);
                    StartRebinding(action, index, true, movementBindingIndex);
                }else{
                    // UPDATE TEXT
                    RebindingComplete(action, index, true, movementBindingIndex);
                }
            })
            .Start();
        }else{
        rebindingOperation = action.PerformInteractiveRebinding()
        .WithControlsExcluding("Mouse")
        .OnMatchWaitForAnother(0.1f)
        .OnComplete((operation)=>{
            
            // check if it is a duplicate
            if(CheckDuplicateBindings(action, index)){
                int bindingIndex = action.GetBindingIndexForControl(action.controls[0]);
                // CLEAN UP
                rebindingOperation.Dispose();

                action.RemoveBindingOverride(bindingIndex);
                StartRebinding(action, index);
            }else{
            // UPDATE TEXT
            RebindingComplete(action, index);
            }
            
            })
        .Start();
        }
        // WE HAVE TO DISPOSE OF THIS ON COMPLETE>.. VERY IMPORTANT ... will mess up ur mem if u dont
    }
    void RebindingComplete(InputAction action, int index,bool isMovement= false, int movementBindingIndex=-1){
        if(isMovement){
            movementText[index].text = InputControlPath.ToHumanReadableString(
            action.bindings[movementBindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
            );
            rebindingOperation.Dispose();
            overlay.SetActive(false);
        }
        else{
        int bindingIndex = action.GetBindingIndexForControl(action.controls[0]);

        
        textValues[index].text = InputControlPath.ToHumanReadableString(
            action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );
        rebindingOperation.Dispose();
        overlay.SetActive(false);
        }
    }
    

    private bool CheckDuplicateBindings(InputAction action, int actionIndex, bool allCompositeParts = false, int movementBindingIndex = -1){
        int bindingIndex;
        InputBinding newBinding;
        if(movementBindingIndex != -1){
            bindingIndex = movementBindingIndex;
            newBinding = action.bindings[bindingIndex];

        }else{
            newBinding = action.bindings[0];
            bindingIndex = actionVars[actionIndex].GetBindingIndexForControl(actionVars[actionIndex].controls[0]);
        }
    
        if (allCompositeParts){
            // CHECKING FOR COMPOSITE PARTS ONLY
            var wasd = Move.ChangeCompositeBinding("WASD");
        
     
            int [] indexes=  new int []{
                wasd.NextPartBinding("Up").bindingIndex,
                wasd.NextPartBinding("Down").bindingIndex,
                wasd.NextPartBinding("Left").bindingIndex,
                wasd.NextPartBinding("Right").bindingIndex
            };
        
            for ( int i = 0; i < indexes.Length ; i++){
                
                
                if (indexes[i] == movementBindingIndex){
                    continue;
                }
                if(Move.bindings[indexes[i]].effectivePath == Move.bindings[movementBindingIndex].effectivePath){
                    Debug.Log("duplicate path");
                    return true;
                }
            }
        }
    
        foreach(InputBinding binding in action.actionMap.bindings){
        
                if (binding.action == newBinding.action){
                    continue;
                }
                if(binding.effectivePath == newBinding.effectivePath){
                    Debug.Log("duplicate path");
                    return true;
                }
        }
        
        // Debug.Log(actions.bindings[controls.Player.crouch.controls[0]]);
       
        return false;
    }

    public InputMaster GetAction(){
        return controls;
    }

    public void OnEnable(){
        LoadSettings();
        
    }
    private void LoadSettings(){
           // LOADING
        mouseSlider.value = PlayerPrefs.GetFloat("sensitivity");
        var rebinds = PlayerPrefs.GetString("rebinds");
        if(!string.IsNullOrEmpty(rebinds)){
            controls.LoadBindingOverridesFromJson(rebinds);
        }
    }
    public void OnDisable(){
        // SAVING
        // var rebinds = controls.SaveBindingOverridesAsJson();
        // PlayerPrefs.SetString("rebinds", rebinds);
    }
    public void SaveBindings(){
 
        var rebinds = controls.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        PlayerPrefs.SetFloat("sensitivity", mouseSlider.value);
        PlayerPrefs.Save();
    }
    private void deleteBindings(){
        PlayerPrefs.DeleteKey("rebinds");
    }
    public void ResetBindings(){
        var rebinds = PlayerPrefs.GetString("rebinds");
        if(!string.IsNullOrEmpty(rebinds)){
            controls.LoadBindingOverridesFromJson(rebinds);
        }
        UpdateText();
    }
    public void UpdateText(){
        int i ;
        for(i =0; i < buttons.Length;i++){
            int bindingIndex = actionVars[i].GetBindingIndexForControl(actionVars[i].controls[0]);

            textValues[i].text = InputControlPath.ToHumanReadableString(
            actionVars[i].bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
            );  
        }
        // /WASD
        var wasd = Move.ChangeCompositeBinding("WASD");
        
     
        int [] bindingIndexes=  new int []{
            wasd.NextPartBinding("Up").bindingIndex,
            wasd.NextPartBinding("Down").bindingIndex,
            wasd.NextPartBinding("Left").bindingIndex,
            wasd.NextPartBinding("Right").bindingIndex
        };
      
        for ( i = 0; i < bindingIndexes.Length ; i++){
            
            movementText[i].text  = InputControlPath.ToHumanReadableString(
                Move.bindings[bindingIndexes[i]].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );
        }
        float split_val = PlayerPrefs.GetFloat("sensitivity");
        mouseSlider.value = split_val;

        
    }
    private void SetUpListeners(){
        // first we build an array// >THIS HAS TO BE EDITED TO MAKE IS USABLE...⚠
        int temp_last = 0;
        for (int i =0; i < buttons.Length; i++){
            int temp = i;
            buttons[i].onClick.AddListener(()=>{
            
                StartRebinding(actionVars[temp], temp);
            });
            temp_last = temp;
        }
        temp_last += 1;
        var bindingIndex = Move.bindings.IndexOf(x=> x.isPartOfComposite && x.name =="Up");
        var wasd = Move.ChangeCompositeBinding("WASD");
        
     
        int [] bindingIndexes=  new int []{
            wasd.NextPartBinding("Up").bindingIndex,
            wasd.NextPartBinding("Down").bindingIndex,
            wasd.NextPartBinding("Left").bindingIndex,
            wasd.NextPartBinding("Right").bindingIndex
        };
      
        for (int i = 0; i < bindingIndexes.Length ; i++){
            int temp = i;
            movementButtons[i].onClick.AddListener(()=>{
                StartRebinding(Move, temp, true,bindingIndexes[temp]);
            });
        }
    // // LISTENERS FOR MOVEMENT
    //     buttonUp.onClick.AddListener(()=>{
    //         StartRebinding(Move, temp_last, true, Move.bindings.IndexOf(x=> x.isPartOfComposite && x.name == "Up") );
    //     });

    }
}
