using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MPOptionsManager : MonoBehaviour
{
    // all the button actions and ui manipulation takes place here 
    // Start is called before the first frame update
    // PROFILE SETUP (ONLINE - OFFLINE DEPENDENT UI)
    public GameObject ProfileSetupOfflineOptions;
    public GameObject ProfileSetupOnlineOptions;
    public Button SinglePlayerButton;
    public int SinglePlayerSwitch;
    // CREATE / LOGIN FORMS 
    [Header("Form Opening Buttons")]
    public Button OpenCreateFormBtn;
    public Button OpenLoginFormBtn;
    public Button OpenForgotPasswordBtn;
    public Button TermsButton;

    [Header("Forms")]
    public GameObject CreateForm;
    public GameObject LoginForm;
    public GameObject PromptObject;
    [Header("NOTIFICATONS")]
    public NotificationsManager Notifications;
    [Header("LOG OUT")]
    public Button LogOutBtn;
    // public GameObject 
    [Header("Status")]
    public GameObject StatusOnlineObject;
    public GameObject StatusOfflineObject;
    public TMP_Text UsernameText;
    // DELEGATES
    void Start()
    {
        ToggleProfileOnline(false);
        AddListeners();
        AutoLogin();
        // LoadUsername();
        UsernameText.text = "loading...";
    }
    void OnEnable(){
        DataManager.OnUpdate += UpdateUsername;
    }
    void OnDisable(){
        DataManager.OnUpdate -= UpdateUsername;
    }

    public void ToggleProfileOnline(bool toggle){
            ProfileSetupOfflineOptions.SetActive(!toggle);
            ProfileSetupOnlineOptions.SetActive(toggle);
    }
    void AddListeners(){
        TermsButton.onClick.AddListener(()=>{
            PromptObject.SetActive(true);
        });
        OpenForgotPasswordBtn.onClick.AddListener(()=>{
            GetComponent<AuthManager>().ChangePassword(PlayerPrefs.GetString("email"));
        });
        LogOutBtn.onClick.AddListener(()=>{
            GetComponent<AuthManager>().LogOutUser();
            ToggleProfileOnline(false);
        });
        SinglePlayerButton.onClick.AddListener(()=>{
            SceneManager.LoadScene(SinglePlayerSwitch);
        });
    }
    void AutoLogin(){
        string mail = PlayerPrefs.GetString("email");
        string pass = PlayerPrefs.GetString("password");
        Debug.Log(mail);
        if(!string.IsNullOrEmpty(mail) && !string.IsNullOrEmpty(pass)){
            GetComponent<AuthManager>().LoginAccount(mail, pass);
        }
    }
    public void ToggleStatus(bool isLoggedIn){
        if(isLoggedIn){
            StatusOnlineObject.SetActive(true);
            StatusOfflineObject.SetActive(false);
        }else{
            StatusOnlineObject.SetActive(false);
            StatusOfflineObject.SetActive(true);
        }
    }
    public void LoadUsername(){
        // UsernameText.text = GetComponent<DataManager>().Username;        
        GetComponent<DataManager>().FetchData();
    } 
    public  void UpdateUsername(){
        UsernameText.text = GetComponent<DataManager>().Username;
    }
}
