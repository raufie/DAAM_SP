using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    public enum CurrentWeapon
    {
       NONE,PISTOL,RIFLE,ROCKET_LAUNCHER,GRENADE
    };
    // Start is called before the first frame update
    public CurrentWeapon currentWeapon;
    public GameObject releaseObject;
    // make a gameobject array of 5 
    public WeaponBase[] weapons;
    public GameObject[] weaponAssets;
    // prefabs

    public GameObject grenadePrefab;
    public GameObject rocketPrefab;
    public GameObject sparkPrefab;
    public AudioManager audioManager;
    public bool isDebug;

    public GameObject LaserPrefab;
    void Awake(){
        
        weapons = new WeaponBase[5];
        
        weapons[0] = new WeaponBase("None",0.5f,GetReleaseObject(0), 0,0,0,0,15,0);
        weapons[1] = new SemiAutomaticWeapon("Pistol",0.1f,GetReleaseObject(1), 15*8,15,15,20 ,15,1);
        weapons[2] = new AutomaticWeapon("Rifle",0.15f,GetReleaseObject(2),30*8,30,30,30,15,1);
        weapons[3] = new RocketLauncherWeapon("RocketLauncher",0.1f,GetReleaseObject(3),1*6,1,1,5,55,0.25f, rocketPrefab, 5.0f, 5.0f);
        weapons[4] = new GrenadeWeapon("Grenade",0.1f,GetReleaseObject(4),1*20,1,1,20,55,0.25f, grenadePrefab, 3.0f, 3.0f);
    }
    void Start()
    {
        SwitchWeapon(1);
        audioManager = GameObject.FindGameObjectsWithTag("AudioManager")[0].GetComponent<AudioManager>();

        
    }
    void Update (){
        weapons[(int)currentWeapon].DrawRay();
        weapons[(int)currentWeapon].IsReloaded();
    }
    // FOR INPUTMAPPER
    public void FireSemiWeapon(){
        
        if (weapons[(int)currentWeapon].IsFireable()){
            InstantiateLaser();
            audioManager.fireSFXEvent(weapons[(int)currentWeapon].Name,GetCurrentReleaseObject().transform.position);
            weapons[(int)currentWeapon].Fire();
            Instantiate(sparkPrefab, GetCurrentReleaseObject().transform.position, Quaternion.Euler(0,180f, 0));
       }
    }
    public void FireAutoWeapon(){
        
         if (weapons[(int)currentWeapon] is AutomaticWeapon){ 
            if (weapons[(int)currentWeapon].IsFireable()){
                InstantiateLaser();
                audioManager.fireSFXEvent(weapons[(int)currentWeapon].Name,GetCurrentReleaseObject().transform.position);
                Instantiate(sparkPrefab, GetCurrentReleaseObject().transform.position, Quaternion.Euler(0,180f, 0));
                weapons[(int)currentWeapon].Fire();

            }
        }        
    }
    public void ReloadWeapon(){
        weapons[(int)currentWeapon].InitiateReloading();

    }

    public void SwitchWeapon(int to){
        currentWeapon = (CurrentWeapon)to;
        SwitchToAsset(to);
        // Debug.Log(currentWeapon);
        // SWITCH THE ASSET
    }
    private void SwitchToAsset(int to){
        for(int i = 0; i < weaponAssets.Length; i++){
            if (to == i){
                weaponAssets[i].SetActive(true);
            }else {
                weaponAssets[i].SetActive(false);
            }
        }
    }
    private GameObject GetReleaseObject(int index){
        return weaponAssets[index].transform.Find("release").gameObject;
    }
    public GameObject GetCurrentReleaseObject(){
        return GetReleaseObject((int)currentWeapon);

    }
    public void giveAmmo(int [] ammoArray){
        
        for (int i = 0; i < ammoArray.Length; i++){
            weapons[i].addAmmo(ammoArray[i]);    
        }
        
    }
    // GUI
    private void OnGUI() {
        if(isDebug){
        GUI.backgroundColor = Color.red;
        WeaponBase weapon = weapons[(int)currentWeapon];
        GUI.Button(new Rect(10, 20, 150, 80),weapon.getName()+ weapon.getCurrentMag()+ "/"+ weapon.getUnloadedAmmo() );
        GUI.Button(new Rect(10, 170, 150, 20),weapon.getStatus() );
        }
    }
    
    public WeaponBase GetCurrentWeapon(){
        return weapons[(int)currentWeapon];
    }
    // MP
    public GameObject FireSemiWeaponMP(){
        
        if (weapons[(int)currentWeapon].IsFireable()){
            
            audioManager.fireSFXEvent(weapons[(int)currentWeapon].Name,GetCurrentReleaseObject().transform.position);
            GameObject obj = weapons[(int)currentWeapon].FireMP();
            Instantiate(sparkPrefab, GetCurrentReleaseObject().transform.position, Quaternion.Euler(0,180f, 0));
            return obj;
       }
       return null;
    }
    public void FireAutoWeaponMP(){
         if (weapons[(int)currentWeapon] is AutomaticWeapon){ 
            if (weapons[(int)currentWeapon].IsFireable()){
                audioManager.fireSFXEvent(weapons[(int)currentWeapon].Name,GetCurrentReleaseObject().transform.position);
                Instantiate(sparkPrefab, GetCurrentReleaseObject().transform.position, Quaternion.Euler(0,180f, 0));
                weapons[(int)currentWeapon].FireMP();

            }
        }        
    }
    public void InstantiateLaser(){
        if (currentWeapon != CurrentWeapon.PISTOL && currentWeapon != CurrentWeapon.RIFLE ){
            return;
        }
        GameObject obj = Instantiate(LaserPrefab, GetCurrentReleaseObject().transform.position, Quaternion.identity);
        obj.GetComponent<LaserManager>().LaunchConnected(GetCurrentReleaseObject().transform.position,weapons[(int)currentWeapon].getCameraHit());
        // Instantiate
        // Set Points
    }

}
