using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Notification : MonoBehaviour
{
    public enum NotificationType {
        LOADING, 
        ERROR,
        SUCCESS
    }
    public NotificationType Type;
    public GameObject LoadingObj;
    public GameObject DangerObj;
    public GameObject SuccessObj;
    public TMP_Text Title;
    public TMP_Text Description;
    private float StartTime;
    public float duration;
    void Start(){
        // UpdateNotification();
        
    }
    void Update(){

    }
    public void Initialize(string label, string description, NotificationType type, float _duration){
        Debug.Log(label);
        Title.text = label;
        Description.text = description;
        Type = type;
        UpdateNotification();
        duration = _duration;
        gameObject.SetActive(true);


    }
    void UpdateNotification(){
        if(Type == NotificationType.LOADING){
            LoadingObj.SetActive(true);
            DangerObj.SetActive(false);
            SuccessObj.SetActive(false);
        }
        else if (Type == NotificationType.ERROR){
            LoadingObj.SetActive(false);
            DangerObj.SetActive(true);
            SuccessObj.SetActive(false);
        }else {
            LoadingObj.SetActive(false);
            DangerObj.SetActive(false);
            SuccessObj.SetActive(true);
        }
    }
}
