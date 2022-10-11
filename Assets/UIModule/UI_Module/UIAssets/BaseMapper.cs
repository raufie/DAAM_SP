using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BaseMapper : MonoBehaviour
{

    public Button SwitchButton;
    public int intendedSwitch;
    // Start is called before the first frame update
    void Start()
    {
        SwitchButton.GetComponent<Button>().onClick.AddListener(switchScene);
    }
    void switchScene(){
        SceneManager.LoadScene(intendedSwitch);
    }
}
