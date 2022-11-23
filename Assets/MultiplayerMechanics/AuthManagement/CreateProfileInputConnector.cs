using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CreateProfileInputConnector :MonoBehaviour
{
    // Text fields
    public TMP_InputField Username;
    public TMP_InputField Password;
    public TMP_InputField RetypedPassword;
    public TMP_InputField Email;
    public TMP_InputField FirstName;
    public TMP_InputField LastName;
    public Toggle Terms;
    [Header("FORM ACTIONS")]
    public Button CreateAccountButton;
    // 
    private AuthValidator Validator;

    // Start is called before the first frame update
    void Start()
    {
        Validator = GetComponent<AuthValidator>();
        CreateAccountButton.onClick.AddListener(CreateAccount);
    }

    void CreateAccount(){
        // check validators
        
        if(Validator.IsNewProfileValid(Username.text, FirstName.text, LastName.text, Email.text, Password.text, RetypedPassword.text, Terms.isOn)){
            GetComponent<AuthManager>().CreateAccount(Username.text, FirstName.text, LastName.text, Email.text, Password.text);
        }
        
    }
    

}
