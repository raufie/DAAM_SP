using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ListManager : MonoBehaviour
{
    public OptionBase [] options;
    public GameObject ListGroup;
    public GameObject ListObjectPrefab;
    public GameObject [] InitializedObjects;
    public Button AcceptBtn;
    public Button RejectBtn;
    public int SelectedIndex = -1;
    public GameObject LoadingDisplay;
    void Start(){
        LoadOptions();
        LoadListGUI();
        AddListeners();
        // StateManager.SaveAsJson("newshit1.dat");
        // StateManager.SaveAsJson("newshit2.dat");        
    }

    void Accept(){
        // call accept of relevant option
        // on load
        if(LoadingDisplay != null){
            LoadingDisplay.SetActive(true);
        }
        StateManager.LoadState(options[SelectedIndex].id);
    }
    void OnEnable(){
        // Start();
    }
    void Reject(){
        if(SelectedIndex != -1){
        // delete the game at the selected index
        options[SelectedIndex].reject();
        Destroy(InitializedObjects[SelectedIndex]);
        StateManager.DeleteState(options[SelectedIndex].id);
        }
    }
    void LoadOptions(){
        LevelData [] levelData = StateManager.GetSaves();
        options = new OptionBase[levelData.Length];
        for (int i =0; i < levelData.Length; i++){
            options[i] = new OptionBase(
                $"Chapter {levelData[i].chapter}",
                levelData[i].name,
                levelData[i].date,
                levelData[i].path
                );
        }
    }
    void LoadListGUI(){
        SelectedIndex = -1;
        InitializedObjects = new GameObject [options.Length];
        for(int i = 0; i< options.Length; i++){
            // Debug.Log(options[i].attr2);
            int temp_i = i;

            GameObject Obj = Instantiate(ListObjectPrefab, new Vector3(0f,0f,0f), Quaternion.identity);
            Obj.transform.SetParent(ListGroup.transform);
            Obj.transform.localScale = new Vector3(1,1,1);

            Obj.GetComponent<ListItem>().setAttribute1(options[i].attr1);
            Obj.GetComponent<ListItem>().setAttribute2(options[i].attr2);
            Obj.GetComponent<ListItem>().setAttribute3(options[i].attr3);
            Obj.GetComponent<ListItem>().setIndex(i);
// attaching btn listeners
            Obj.GetComponent<ListItem>().btn.onClick.AddListener(()=>{
                SelectedIndex = temp_i;
            });

            InitializedObjects[i]=Obj;

        }
    }
    void RemoveOption(){
        // Debug.Log("removing");
    }

    void AddListeners(){
            AcceptBtn.onClick.AddListener(Accept);
            RejectBtn.onClick.AddListener(Reject);
    }
}
