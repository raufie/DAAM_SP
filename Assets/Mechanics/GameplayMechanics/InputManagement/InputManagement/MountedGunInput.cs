using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountedGunInput : MonoBehaviour
{
    public MountedWeaponManager weaponsManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void disable(){
        weaponsManager.enabled = false;
    }
    public void enable(){
        weaponsManager.enabled = true;
    }
    public void SetCurrent(GameObject curr){
        weaponsManager = curr.GetComponent<MountedWeaponManager>();
    }
}
