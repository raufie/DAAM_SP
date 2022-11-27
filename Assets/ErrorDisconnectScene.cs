using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ErrorDisconnectScene : MonoBehaviour
{
    public int MenuScene;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        button.onClick.AddListener(()=>{
            SceneManager.LoadScene(MenuScene);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
