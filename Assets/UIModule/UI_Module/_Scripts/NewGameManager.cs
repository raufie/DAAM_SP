using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NewGameManager : MonoBehaviour
{
    public Button [] Difficulties = new Button[3];
    public Button StartButton;
    private int selectedDiff;
    void Start(){
        for (int i = 0; i < Difficulties.Length; i++){
            int tempI = i;
            Difficulties[i].onClick.AddListener(()=>{
                selectedDiff = tempI;
            });
        }

        StartButton.onClick.AddListener(()=>{
            StateManager.StartNew(selectedDiff);
        });
    }
    
}
