using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    // Start is called before the first frame update

    public int pistolAmmo;
    public int rocketAmmo;
    public int grenadeAmmo;
    public int rifleAmmo;
    public int [] ammoArray;
    void Start(){
        ammoArray = new int[5];
        ammoArray[0] = 0;
        ammoArray[1] = pistolAmmo;
        ammoArray[2] = rifleAmmo;
        ammoArray[3] = rocketAmmo;
        ammoArray[4] = grenadeAmmo;
    }
    private void OnTriggerEnter(Collider other){
        Debug.Log(other.tag);
        if(other.tag == "Player"){
            Debug.Log(other.gameObject);
            other.gameObject.GetComponent<WeaponsManager>().giveAmmo(ammoArray);
            Destroy(gameObject);
        }
    }
}
