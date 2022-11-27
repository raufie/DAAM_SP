using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BaseMapper : MonoBehaviour
{

    public Button SwitchButton;
    public int intendedSwitch;
    public GameObject LoadingDisplay;
    // Start is called before the first frame update
    void Start()
    {
        SwitchButton.GetComponent<Button>().onClick.AddListener(switchScene);
    }
    void switchScene(){
        if(LoadingDisplay != null){
            LoadingDisplay.SetActive(true);
        }
        SceneManager.LoadScene(intendedSwitch);
    }
}
