using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
 public int score;
 
 public string ToJson(){
    return JsonUtility.ToJson(this);
 }

 public void LoadFromJson(string json){
    JsonUtility.FromJsonOverwrite(json, this);
 }

}
