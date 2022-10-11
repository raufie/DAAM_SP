using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    public GameObject [] Collectibles;
    public void LoadFromCode(string s){
        Debug.Log(s);
        for(int i = 0;i < s.Length;i++){
            if(s[i] == 'a'){
                Collectibles[i].SetActive(true);
            }else{
                Destroy(Collectibles[i]);
            }
        }
    }
    public string GetCode(){
        string s = "";
        for(int i = 0; i < Collectibles.Length ; i++){
            if(Collectibles[i] == null){
                s+="d";
            }else {
                s+= "a";
            }
        }
        return s;

    }
}
