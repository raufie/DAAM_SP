using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    [Header("0: Player; 1: Mounted Guns")]
    public GameObject [] Cameras ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMountedGunCam(GameObject cam){
        if(Cameras.Length > 1){
            Cameras[1] = cam;
        }
    }
    public void SwitchCams(int id){
        for(int i = 0; i < Cameras.Length; i++){
            if(i != id){
                Cameras[i].SetActive(false);
            }else{
                Cameras[i].SetActive(true);
            }
        }
    }

}
