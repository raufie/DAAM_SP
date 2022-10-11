using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUDManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text health;
    public PlayerObject playerObject;
    // MILESTONES AND INFO
    public GameObject MilestoneSprite;
    public GameObject MilestoneInformation;
    public MilestonesManager milestones;
    public float MilestonePadding = 0.05f;
    public bool IsViewingInfo = false;
    public TMP_Text info_label;
    public TMP_Text info_desc;



    // INTERNAL
    private InputMaster controls;

    void Awake(){
        RefreshInput();
    }    
    // Update is called once per frame
    void FixedUpdate()
    {
        health.text = ""+ playerObject.Health;
        // THE BELOW IS SUBJECT TO CHANGE
        if(controls.Player.Info.ReadValue<float>()>=0.5f){
            IsViewingInfo = true;
        }else{
            IsViewingInfo = false;
        }
        if(IsViewingInfo){
        ShowMilestonePoint();
        }else{
            MilestoneSprite.SetActive(false);
            MilestoneInformation.SetActive(false);
        }

        // MilestoneSprite.GetComponent< = screenPos;
    }
    void ShowMilestonePoint(){
        
        MilestoneInformation.SetActive(true);
        
        
        GameObject milestone = milestones.GetCurrentMilestone();
        Vector3 milestonePosition;
        if(milestone == null){
            MilestoneSprite.SetActive(false);
            info_label.text = "You don't have any remaining Milestones";
            info_desc.text = "";
            return;
        }else{
            milestonePosition = milestone.transform.position;
            info_label.text = milestone.GetComponent<MilestoneBase>().Label;
            info_desc.text = milestone.GetComponent<MilestoneBase>().Description;
        }
        
        MilestoneSprite.SetActive(true);
        Vector3 viewPortPoint = Camera.main.WorldToViewportPoint(milestonePosition);
        Debug.Log(viewPortPoint);
        
        Vector2 screenPos = Camera.main.WorldToScreenPoint(milestonePosition);
        RectTransform sprite = MilestoneSprite.GetComponent<RectTransform>();

        Vector3 ScreenPosition = new Vector3(1,1,0);

        if(viewPortPoint.z < 0f){
            viewPortPoint.x*=-1f;
            viewPortPoint.y*=-1f;
        }

        float pointX = Mathf.Clamp(viewPortPoint.x, 0f+MilestonePadding, 1f-MilestonePadding);
        float pointY = Mathf.Clamp(viewPortPoint.y, 0f+MilestonePadding, 1f-MilestonePadding);

        ScreenPosition.x = pointX;
        ScreenPosition.y = pointY;
        ScreenPosition = Camera.main.ViewportToScreenPoint(ScreenPosition);        
        sprite.position = ScreenPosition;
    }
    private void RefreshInput(){
        
        controls = GameObject.FindGameObjectsWithTag("InputManagement")[0].GetComponent<InputManagement>().GetActions();
 
        if (controls == null){
            controls = new InputMaster();
        }
        controls.Enable();
    }
}
