using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AchievementsManager : MonoBehaviour
{
    public TMP_Text Kills;
    public TMP_Text Deaths;
    public TMP_Text KD;
    public DataManager dataManager;
    // Start is called before the first frame update
    void Start()
    {
        DataManager.OnUpdate+=OnDataFetch;
    }

    void OnEnable(){
        dataManager.FetchStatistics();
    }
    public void OnDataFetch(){
        Deaths.text = ""+dataManager.Deaths;
        Kills.text = ""+dataManager.Kills;
        KD.text = ""+dataManager.KD;        
    }
}
