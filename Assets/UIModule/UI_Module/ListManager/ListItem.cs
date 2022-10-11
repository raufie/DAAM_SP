using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ListItem : MonoBehaviour
{
    public Button btn;
    public TextMeshProUGUI Attribute1;
    public TextMeshProUGUI Attribute2;
    public TextMeshProUGUI Attribute3;
    private int index;
    
    
    
    public void setAttribute1(string attr){
        Attribute1.text = attr;
    }
    
    public void setAttribute2(string attr){
        Attribute2.text = attr;
    }
    
    public void setAttribute3(string attr){
        Attribute3.text = attr;

    } 
    public void setIndex(int _index){
        index = _index;
    }
    
    public int getIndex(){
        return index;
    }

    
}
