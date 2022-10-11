using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TabToggler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject [] options;
    public Button [] tabs;
    private int currentTab ;
    public static int curr;

    // Setting sprites for tabs
    public Sprite ActiveTabSprite;
    public Sprite InactiveTabSprite;

    public bool UpdateSprite = false;
    void Start()
    {
            currentTab = 0;
            int i = 0;
            while(i < tabs.Length){
                int j = i;
                tabs[j].GetComponent<Button>().onClick.AddListener(()=>{toggleToTab(j);});
                i++;
            }          
            if(UpdateSprite){
                setCurrentTab(0);
            }
    }


    void toggleToTab(int index) {
        setCurrentTab(index);
        TabToggler.curr = index;
        for (int i = 0; i < options.Length ; i++){
            if (index != i){
                options[i].SetActive(false);
            }else{
                options[i].SetActive(true);
            }
        }
        Debug.Log("switched to " + currentTab);
    }
    void setCurrentTab(int to){
        currentTab = to;
        // others should be come inactive
        if(!UpdateSprite){
            return;
        }
        for(int i = 0; i < tabs.Length;i++){
            if(i == to){
                // Active
                tabs[i].gameObject.GetComponent<Image>().sprite = ActiveTabSprite;
            }else{
                // Inactive
                tabs[i].gameObject.GetComponent<Image>().sprite = InactiveTabSprite;
            }

        }
    }
    public int getCurrentTab(){
        return currentTab;
    }

}
