using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.MultiplayerModels;
using TMPro;

public class AuthManager:MonoBehaviour
{
    private NotificationsManager Notifications;
    private string Username;
    private string Email;
    private string Password;
    private string FirstName;
    private string LastName;
    public GameObject LoginForm;

    
    // public TMP_Inpu
    // Start is called before the first frame update
    void Awake()
    {
        Notifications = GetComponent<MPOptionsManager>().Notifications;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateAccount(string username, string fname, string lname, string email, string password ){
        Username = username;
        Password = password;
        Email = email;
        FirstName = fname;
        LastName = lname;

        Notifications.ShowAlert("Creating Profile", "", Notification.NotificationType.LOADING, 100f);
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest(){
                DisplayName = Username,
                Username = Username,
                Email = email,
                Password = password,
                RequireBothUsernameAndEmail = true
            },
            OnCreateAccountSuccess,
            OnCreateAccountReject
        );
    }
    public void OnCreateAccountSuccess(RegisterPlayFabUserResult response){
      
        var request = new LoginWithEmailAddressRequest {
            Email  = Email,
            Password = Password
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, res=>{
        },
        OnCreateAccountReject);
        

        // LOGIN THE PLAYER
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string, string>() {
                {"FirstName", FirstName},
                {"LastName", LastName},
                {"DisplayName", Username},
                {"Model", "0"}
            }
        }, response=>{
            Notifications.ShowAlert("Profile successfully created", "Please go to profile setup and Log In to your account", Notification.NotificationType.SUCCESS, 5f);
        },OnCreateAccountReject);
        // LOGIN USER... SAVE THE PASSWORD INFO AUTOMATICALLY
    }
    public void OnCreateAccountReject(PlayFabError error){
        Notifications.ShowAlert("Error creating profile", error.ErrorMessage, Notification.NotificationType.ERROR, 5f);
        GetComponent<MPOptionsManager>().ToggleStatus(false);
    }
    public void LoginAccount(string email, string password){
        Notifications.ShowAlert("Logging In", "", Notification.NotificationType.LOADING, 50f);
        var request = new LoginWithEmailAddressRequest {
            Email  = email,
            Password = password
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, res=>{
        
        PlayerPrefs.SetString("email",email);
        PlayerPrefs.SetString("password",password);
        PlayerPrefs.Save();

        Notifications.ShowAlert("Profile Logged in successfully", "", Notification.NotificationType.SUCCESS, 1f);
        GetComponent<MPOptionsManager>().ToggleProfileOnline(true);
        GetComponent<MPOptionsManager>().LoadUsername();
        GetComponent<MPOptionsManager>().ToggleStatus(true);
        LoginForm.SetActive(false);
        },
        error=>{
        Notifications.ShowAlert("Error Logging in", error.ErrorMessage, Notification.NotificationType.ERROR, 5f);
        GetComponent<MPOptionsManager>().ToggleStatus(false);
        LoginForm.SetActive(true);

        }
        );
        
        
    }
    public void ChangePassword(string email){
        Notifications.ShowAlert("Sending Recovery Email", "", Notification.NotificationType.LOADING, 100f);
        var Request = new SendAccountRecoveryEmailRequest{
            Email = email,
            TitleId = "D84E3"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(Request, res=>{
            Notifications.ShowAlert("Recovery Email Sent to your email", "Please check your email to change password", Notification.NotificationType.SUCCESS, 1f);
        },
        error=>{
            Notifications.ShowAlert("Error sending recovery email", error.ErrorMessage,  Notification.NotificationType.ERROR, 5f);
        }
        );
    }
    public void LogOutUser(){
        PlayerPrefs.DeleteKey("email");
        PlayerPrefs.DeleteKey("password");
        PlayerPrefs.Save();
        GetComponent<MPOptionsManager>().ToggleStatus(false);
    }
    public void GetUserStats(){
        
    }
}
