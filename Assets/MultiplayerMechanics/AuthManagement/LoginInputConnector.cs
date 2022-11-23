using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoginInputConnector : MonoBehaviour
{
    public Button LoginButton;
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;
    void Start(){
        LoginButton.onClick.AddListener(LoginAccount);
    }
    void LoginAccount(){
        if(GetComponent<AuthValidator>().IsLoginValid(EmailInput.text.Trim(), PasswordInput.text.Trim())){
            // Go ahead login
            GetComponent<AuthManager>().LoginAccount(EmailInput.text.Trim(), PasswordInput.text.Trim());
        }   

    }
}
