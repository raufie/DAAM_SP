using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProfileSetupScript : MonoBehaviour
{
    public Button loginBtn;
    public Button createAccountBtn;
    public GameObject loginForm;
    public GameObject createAccountForm;
    // Start is called before the first frame update
    void Start()
    {
    loginBtn.GetComponent<Button>().onClick.AddListener(openLoginForm);
    createAccountBtn.GetComponent<Button>().onClick.AddListener(openAccountForm);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void openLoginForm(){
        gameObject.SetActive(false);
        loginForm.SetActive(true);
    }
    void openAccountForm(){
        gameObject.SetActive(false);
        createAccountForm.SetActive(true);
    }
}
