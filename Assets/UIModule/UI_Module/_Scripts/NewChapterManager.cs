using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NewChapterManager : MonoBehaviour
{
    // UI assignments
    public Button LeftBtn;
    public Button RightBtn;
    public Button StartBtn;
    
    public TMP_Text TitleText;
    public TMP_Text ChapterText;

    public Image ChapterImage;

    public Sprite [] sprites = new Sprite[10];

    public TMP_Dropdown DifficultyDropdown;

    public string [] chapterTitles = new string[10];


    // STATES
    private int currentState;

    public void Start(){
        RightBtn.onClick.AddListener(NextChapter);
        LeftBtn.onClick.AddListener(PreviousChapter);
        StartBtn.onClick.AddListener(LoadChapter);
    }
    public void NextChapter(){
        if(currentState <9){
            currentState+=1;
            SetUIState();
        }
    }
    public void PreviousChapter(){
        if(currentState>0){
            currentState-=1;
            SetUIState();
        }
    }
    public void SetUIState(){
        ChapterText.text = "Chapter "+ (currentState+1);
        TitleText.text = chapterTitles[currentState];
        ChapterImage.sprite = sprites[currentState];
    }
    public void LoadChapter(){
        StateManager.StartNew(DifficultyDropdown.value, currentState+1);
    }
}
