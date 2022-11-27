using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;

public class DataManager : MonoBehaviour
{
    public string Username;
    public int Model;
    public int Kills;
    public int Deaths;
    public float KD;
    public NotificationsManager Notifications;
    // DELEGATES AND EVENTS
    public delegate void UpdatedAction();
    public static event UpdatedAction OnUpdate;

    void Awake(){
        try{
            Notifications = GetComponent<MPOptionsManager>().Notifications;
        }catch {
            // Notifications = AddComponent<Notifications>();
        }
        
    }
    public void FetchData(){
        Username = "loading...";
        OnUpdate();
        var request = new GetUserDataRequest {

         };
        PlayFabClientAPI.GetUserData(request, res=>{
            Username = res.Data["DisplayName"].Value;
            string s = res.Data["Model"].Value;
            Model = int.Parse(s);
            OnUpdate();
        }, error=>{
            Username = "Error";
            OnUpdate();
        });
        return ;
    }
    public void FetchStatistics(){
        Notifications.ShowAlert("getting stats", "", Notification.NotificationType.LOADING, 200f);
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            (result)=>{
                Notifications.ShowAlert("Success", "Fetched Data Successfully", Notification.NotificationType.SUCCESS, 0.5f);

                foreach (var eachStat in result.Statistics){
                    if(eachStat.StatisticName == "kills"){
                        Kills = eachStat.Value;
                    }
                    if(eachStat.StatisticName == "deaths"){
                        Deaths = eachStat.Value;
                    }
                    
                }
                if(Deaths!= 0){
                    KD = (float)Kills/(float)Deaths;
                    // Debug.Log(KD);
                }
                    
                OnUpdate();
            },
            (error) => {
                Notifications.ShowAlert("ERROR", "Error Fetching data", Notification.NotificationType.ERROR, 0.5f);
                Debug.LogError(error.GenerateErrorReport());
                }
        );

    }
    public void UpdatePlayerData(string _username, int _model){
        PlayerPrefs.SetInt("model",_model);
         PlayerPrefs.SetString("username",_username);
        Notifications.ShowAlert("Updating Data", "", Notification.NotificationType.LOADING, 200f);
        if(!GetComponent<AuthValidator>().IsUsernameValid(_username)){
            Notifications.ShowAlert("Invalid Username", "Please keep it above or 8 characters", Notification.NotificationType.SUCCESS, 1.5f);
            return;
        }
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string, string>() {
                {"DisplayName", _username},
                {"Model", ""+_model}
            }
        }, response=>{
            Notifications.ShowAlert("Successfully updated data", "", Notification.NotificationType.SUCCESS, 1.5f);
            FetchData();
        },(error)=>{
            Notifications.ShowAlert("Error updating data", "", Notification.NotificationType.ERROR, 5f);
            OnUpdate();
        });
    }
    public static void AddKill(){
        // GET KILLS
    PlayFabClientAPI.UpdatePlayerStatistics( new UpdatePlayerStatisticsRequest {
        // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
        Statistics = new List<StatisticUpdate> {
            new StatisticUpdate { StatisticName = "kills", Value = 1 },
        }
    },
    result => { Debug.Log("User statistics updated"); },
    error => { Debug.LogError(error.GenerateErrorReport()); });

    }
    public static void AddDeath(){
        // GET KILLS
    PlayFabClientAPI.UpdatePlayerStatistics( new UpdatePlayerStatisticsRequest {
        // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
        Statistics = new List<StatisticUpdate> {
            new StatisticUpdate { StatisticName = "deaths", Value = 1 },
        }
    },
    result => { Debug.Log("User statistics updated"); },
    error => { Debug.LogError(error.GenerateErrorReport()); });

    }
    

}
