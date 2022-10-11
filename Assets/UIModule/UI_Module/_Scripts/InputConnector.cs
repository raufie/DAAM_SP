using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputConnector : MonoBehaviour
{
    public UIActions uiActions;
    public UIStates uiStates;
    [Header("Main Screen Action Buttons")]
    public Button Multiplayer;
    public Button ResumeBtn;
    public Button ResumeInGameBtn;
    public Button NewGame;
    public Button LoadGame;
    public Button SelectChapter;
    public Button Settings;
    public Button Quit;
    public SwitchManager switchManager;
    [Header("Audio Settings options")]

    public Slider MasterSlider;
    public Slider SFXSlider;
    public Slider MusicSlider;
    public Button SaveChangesBtn;
    public Button DiscardChangesBtn;
    // slider.value
    // Start is called before the first frame update
    void Start()
    {
        uiActions = GetComponent<UIActions>();
        assignAudioListeners();   
        updateAudioText();
        assignMenuListeners();
    }


    void assignAudioListeners(){
        updateStateChanges();
        SaveChangesBtn.onClick.AddListener(()=>saveChanges());
        DiscardChangesBtn.onClick.AddListener(discardChanges);

    }
    void saveChanges(){
        // based on tab counter
      
        if(TabToggler.curr == 0){
            Debug.Log("saving Audio Changes");
            uiActions.saveAudioSettings(MasterSlider.value, SFXSlider.value, MusicSlider.value);
        }
        else if (TabToggler.curr == 1){
            Debug.Log("saving Video changes");
            uiActions.saveVideoSettings();
            
        }
        else if (TabToggler.curr == 2){
            Debug.Log("Saving Controls");
             uiActions.saveControlSettings();
        }
        
    }
    public void updateAudioText(){
            MasterSlider.value = PlayerPrefs.GetFloat("masterVolume");  
            SFXSlider.value = PlayerPrefs.GetFloat("sfxVolume");
            MusicSlider.value = PlayerPrefs.GetFloat("bgmVolume");
    }
    void discardChanges(){
        if(TabToggler.curr == 0){
            updateAudioText();
        }
        else if (TabToggler.curr == 1){
           
           uiActions.discardVideoSettings();
            
        }
        else if (TabToggler.curr == 2){
            
            uiActions.discardControlSettings();
        }
    }
    public void updateStateChanges(){

    }
    void assignMenuListeners(){
        Quit.onClick.AddListener(()=>{
            Application.Quit();
            Debug.Log("qutting ");
            });
        if (ResumeBtn != null){
        ResumeBtn.onClick.AddListener(ResumeGame);
        }
        if (ResumeInGameBtn != null){
        ResumeInGameBtn.onClick.AddListener(ResumeGameInGame);
        }
    }
    void ResumeGame(){
        StateManager.ResumeState();
    }
    void ResumeGameInGame(){
        switchManager.Switch();
    }
}
