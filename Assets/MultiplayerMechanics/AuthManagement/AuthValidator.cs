using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class AuthValidator :MonoBehaviour
{
    private NotificationsManager Notifications;
    public const string MatchEmailPattern =
            @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
              + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
    // Start is called before the first frame update
    void Start()
    {
        Notifications = GetComponent<MPOptionsManager>().Notifications;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IsPasswordValid(string password){
        password = password.Trim();
        Debug.Log(password.Length);
        if(password.Length >=8){
            return true;
        }else{
            Notifications.ShowAlert("Password Must have more than 7 characters", "", Notification.NotificationType.ERROR, 1.5f);
            return false;
        }
    }
    public bool IsNameValid(string name){
        name = name.Trim();
        if(name.Length >= 4){
            return true;
        }
        Notifications.ShowAlert("Name Must have more than 3 characters", "", Notification.NotificationType.ERROR, 1.5f);
        return false;
    }
    public bool IsUsernameValid(string username){
        username = username.Trim();
        if(username.Length >= 8){
            return true;
        }
        Notifications.ShowAlert("Username Must have more than 7 characters", "", Notification.NotificationType.ERROR, 1.5f);
        return false;
        
    }

    public bool IsEmailValid(string email)
    {
        if (email != null) {
            bool IsValid =  Regex.IsMatch(email, MatchEmailPattern);
            if(!IsValid){
                Notifications.ShowAlert("Email is InValid", "", Notification.NotificationType.ERROR, 1.5f);
                return false;
            }else{
                return true;
            }
        }
        else {
            Notifications.ShowAlert("Email is InValid", "", Notification.NotificationType.ERROR, 1.5f);
            return false;
        }
    }
    public bool ArePasswordsEqual(string password, string retypedPassword){
        bool isEqual =  password.Equals(retypedPassword);
        if(isEqual){
            return true;
        }
        Notifications.ShowAlert("Password and retyped passwords do not match", "", Notification.NotificationType.ERROR, 1.5f);
        return false;
    }
    public bool AreTermsAccepted(bool isOn){
        if(isOn){
            return true;
        }
        Notifications.ShowAlert("Please Read and Accept the terms and conditions to continue", "", Notification.NotificationType.ERROR, 1.5f);
        return false;
    }
    public bool IsNewProfileValid(string username, string fname, string lname, string email, string password, string retypedPassword, bool accepted ){
       return IsUsernameValid(username) && IsNameValid(fname) && IsNameValid(lname) && IsEmailValid(email) && IsPasswordValid(password) && ArePasswordsEqual(password, retypedPassword) && AreTermsAccepted(accepted);
    }
// LOGIN VALIDATIOn
    public bool IsLoginValid(string email, string password){
        return IsEmailValid(email) && IsPasswordValid(password);
    }
}
