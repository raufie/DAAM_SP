using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilestoneBase: MonoBehaviour
{
    public string Label;
    public string Description;
    public bool isFinished;
    public GameObject Player;

    [Header("Visuals Management")]
    public MileStoneVisualsManager VisualsManager;

    void Start(){
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        VisualsManager.text.text = Label;
    }
    void FixedUpdate(){
        if (CheckFinished()){
            isFinished = true;
        }
    }
    public virtual bool CheckFinished(){
        return false;
    }
    public void Activate(){
        gameObject.SetActive(true);
    }
}
