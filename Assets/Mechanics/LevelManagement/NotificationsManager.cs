using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationsManager : MonoBehaviour
{
    public Notification notification;
    private float startTime;
    void Start(){

    }
    void Update(){
        if (Time.realtimeSinceStartup > notification.duration + startTime  ){
            // cancel it
            notification.gameObject.SetActive(false);
        }
    }
    public void ShowAlert(string label, string description, Notification.NotificationType type, float duration){
        notification.Initialize(label, description, type, duration);
        startTime = Time.realtimeSinceStartup;
        notification.gameObject.SetActive(true);
    }
}
