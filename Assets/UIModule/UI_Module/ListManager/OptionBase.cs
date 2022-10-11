using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionBase
{
    public string attr1;
    public string attr2;
    public string attr3;
    public string id;


    public OptionBase (string _attr1, string _attr2, string _attr3,string _id){
        attr1 = _attr1;
        attr2 = _attr2;
        attr3 = _attr3;
        id = _id;
    }
    public void accept(){
        Debug.Log("here I will do what i must...");
    }
    public void reject(){
        Debug.Log("here I will reject you.."+" DELETING "+ id);
    }



}
