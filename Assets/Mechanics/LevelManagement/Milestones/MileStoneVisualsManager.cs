using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MileStoneVisualsManager : MonoBehaviour
{
    private GameObject Player;
    public GameObject LabelObject;
    public GameObject SpriteObject;
    public TMP_Text text;
    public float SpriteVisibilityRadius = 20f;
    public float LabelVisibilityRadius = 10f;
    public bool isViewing;

    // Update is called once per frame
    void Update()
    {
        LookAtCam(SpriteObject);
        LookAtCam(LabelObject);

        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        if(isViewing){
            SpriteObject.SetActive(true);
            LabelObject.SetActive(true);
        }
        else if ( distance < SpriteVisibilityRadius){
            SpriteObject.SetActive(true);
            if(distance < LabelVisibilityRadius){
                LabelObject.SetActive(true);
            }else{
                LabelObject.SetActive(false);
            }
        }
        else{
            SpriteObject.SetActive(false);
            LabelObject.SetActive(false);
        }
    }
    void LookAtCam(GameObject obj){
        Vector3 pos = Camera.main.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos, Vector3.up);
        // Vector3 Euler = rotation.eulerAngles;
        // Euler.y+=180f;
        rotation.eulerAngles= new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y+180f, rotation.eulerAngles.z);
        obj.transform.rotation = rotation;
    }
}
