using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ModelSelectorManager : MonoBehaviour
{
    // UI assignments
    public Button LeftBtn;
    public Button RightBtn;
    
    public TMP_Text TitleText;

    public Image ModelImage;

    public Sprite [] sprites = new Sprite[4];

    public string [] modelNames = new string[4];

    // STATES
    public int currentState;

    public void Start(){
    

        RightBtn.onClick.AddListener(NextModel);
        LeftBtn.onClick.AddListener(PreviousModel);
        SetUIState();
    }
    public void NextModel(){
        if(currentState <3){
            currentState+=1;
            SetUIState();
        }
    }
    public void PreviousModel(){
        if(currentState>0){
            currentState-=1;
            SetUIState();
        }
    }
    public void SetUIState(){
        TitleText.text = modelNames[currentState];
        ModelImage.sprite = sprites[currentState];
    }
    public void SetState(int state){
        if(state >=0 && state <=3){
            currentState = state;
            SetUIState();
        }
    }

}
