using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class SuccessEndGame : MonoBehaviour
{
    public TMP_Text t1score;
    public TMP_Text t2score;
    public int MainMenu;
    public Button btn;
    // Start is called before the first frame update
    public float WaitTime = 5f;
    private float StartTime;
    void Start()
    {
        StartTime = Time.time;
        t1score.text = ""+PlayerPrefs.GetInt("t1score");
        t2score.text =""+ PlayerPrefs.GetInt("t2score");
        btn.onClick.AddListener(()=>{
            SceneManager.LoadScene(MainMenu);
        });
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time > StartTime + WaitTime){
            SceneManager.LoadScene(MainMenu);
        }
        
    }
}
