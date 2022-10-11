using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilestonesManager : MonoBehaviour
{
    public MilestoneBase [] Milestones;
    public int CurrentMilestone;
    public bool isDebug;
    void Start(){
        if(isDebug ){
            for(int i =0; i < Milestones.Length;i++){
                
                    Milestones[i].VisualsManager.isViewing = true;   
            }
        }
        
    }
    public void SwitchToMilestone(int milestone){
        if(milestone >= Milestones.Length){
            CurrentMilestone = milestone;
            return;
        }
        for(int i = 0; i < milestone;i++){
            Destroy(Milestones[i]);
        }
        if(milestone < 1){
            CurrentMilestone = 0;
            return;
        }

        
        CurrentMilestone = milestone - 1;
        CompleteMilestone();
    }
    void FixedUpdate(){
        CheckForUpdates();
        
    }
    void CompleteMilestone(){
        if(Milestones[CurrentMilestone]!= null){
        Destroy(Milestones[CurrentMilestone].gameObject);
        }    
    

        CurrentMilestone +=1;
        if(Milestones[CurrentMilestone] == null){
            CurrentMilestone++;
            return;
        }

        if(CurrentMilestone < Milestones.Length){
            Milestones[CurrentMilestone].isFinished = false;
            Milestones[CurrentMilestone].Activate();
        }
       
    }
    void CheckForUpdates(){
        if (CurrentMilestone != Milestones.Length && (Milestones[CurrentMilestone] == null || Milestones[CurrentMilestone].isFinished)){
            CompleteMilestone();
            
        }else if (CurrentMilestone == Milestones.Length){
            Debug.Log("ALL MILESTONES COMPLETED");
        }
    }
    public GameObject GetCurrentMilestone(){
        if (CurrentMilestone >= Milestones.Length ){
            return null;
        }
        return Milestones[CurrentMilestone].gameObject;
        
    }

}
