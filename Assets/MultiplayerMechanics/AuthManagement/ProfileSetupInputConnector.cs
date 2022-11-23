using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ProfileSetupInputConnector : MonoBehaviour
{
    public TMP_InputField DisplayName;
    public ModelSelectorManager ModelManager;
    public Button SaveBtn;
    public Button CancelBtn;
    void Start(){
        AddListeners();
    }
    void OnEnable(){
        DataManager.OnUpdate+=UpdatePlayerData;
    }
    void OnDisable(){
        DataManager.OnUpdate-=UpdatePlayerData;
    }
    void ResetToPrevious(){
        DisplayName.text = GetComponent<DataManager>().Username;
        // ModelManager.
    }
    void AddListeners(){
        SaveBtn.onClick.AddListener(()=>{
            GetComponent<DataManager>().UpdatePlayerData(DisplayName.text,ModelManager.currentState );
        });
        CancelBtn.onClick.AddListener(()=>{
            UpdatePlayerData();
        });
    }
    void UpdatePlayerData(){
        DisplayName.text = GetComponent<DataManager>().Username;
        ModelManager.SetState(GetComponent<DataManager>().Model);
    }
}
