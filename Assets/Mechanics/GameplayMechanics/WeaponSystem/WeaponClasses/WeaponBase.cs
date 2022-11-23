using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponBase 
{
// WEAPON DEFAULTS
    protected float FireRate; // how many seconds of wait before each shot
    public string Name;
    public GameObject releaseObject;
    protected float reloadTime ; 
// AMMO ATTRIBUTES (DEFAULTS AND STATES)
    protected int maxAmmo;
    protected int magazineSize;
    public int currentLoadedAmmo;
    public int currentNonLoadedAmmo;


// OBJECT STATES
    // DAMAGE ATTRIBUTES
    protected int damagePoints;
    
    // weapon states
    public float lastFiredTime = 0f;
    public bool isReloading = false;
    public float lastReloadTime = 0f;



    // liufCamera.main
    void Update(){
        DrawRay();
    }
    
    // CONSTRUCTOR
    public WeaponBase(
        string _Name, 
        float _FireRate, 
        GameObject _releaseObject,
        int _maxAmmo,
        int _magazineSize,
        int _currentLoadedAmmo,
        int _currentNonLoadedAmmo,
        int _damagePoints,
        float _reloadTime
    )
    {
        Name = _Name;
        FireRate = _FireRate;
        releaseObject = _releaseObject;
        
        maxAmmo = _maxAmmo;
        magazineSize = _magazineSize;
        currentLoadedAmmo = _currentLoadedAmmo;
        currentNonLoadedAmmo = _currentNonLoadedAmmo;
        damagePoints = _damagePoints;
        
        reloadTime = _reloadTime;

    }

    


    // OPERATIONAL METHODS ( OVERRIDE AND CALL BASE AS WELL {at the end of doing ur business})
    public virtual void Fire() {
        if (IsFireable()){
        lastFiredTime = Time.time;
        currentLoadedAmmo -=1;
        }
    }

    public void InitiateReloading(){
        if (IsReloadable()){
            lastReloadTime = Time.time;
            isReloading = true;
        }
    }
    public virtual void Reload() {

            int ammoToFetch = magazineSize - currentLoadedAmmo;
            if (currentNonLoadedAmmo < ammoToFetch){
                currentLoadedAmmo += currentNonLoadedAmmo;
                currentNonLoadedAmmo  = 0;
            }else{
                // in the mag >= toFetch
                currentLoadedAmmo += ammoToFetch;
                currentNonLoadedAmmo -= ammoToFetch;
           }      

    }
    
    // utility methods required only by this class and its sub class objects
    public Vector3 GetMouseDirection()
    {
        // // Vector3 mousePos = Mouse.current.position.ReadValue();
      
        Vector3 worldPos = getCameraHit();
        Vector3 direction;
        if(worldPos == Vector3.zero){
            // pos should be very very far away

            Vector3 mousePos = new Vector3(Screen.width/2, Screen.height/2, 10);
            worldPos = Camera.main.gameObject.transform.forward.normalized*10000f;
            direction = worldPos;
        }else{
            direction = (worldPos - releaseObject.transform.position).normalized;
        }
        
        // set release object to look at mouse
        releaseObject.transform.rotation = Quaternion.LookRotation(direction);
        return direction;
    }
    public Vector3 getCameraHit(){
        RaycastHit hit;
        GameObject cam = Camera.main.gameObject;

        if(!Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity)){
            Vector3 unitVec = cam.transform.forward.normalized;

            return unitVec*float.MaxValue;
        }
        
        RaycastHit []hits;
        hits = Physics.RaycastAll(cam.transform.position, cam.transform.forward, Mathf.Infinity);

        // ignore distance means if the camera hits the playr (which we should ignore) we wont hit player or anything within that ignore distance
        Vector3 ignoreVector = new Vector3(releaseObject.transform.position.x+5f, releaseObject.transform.position.y+5f, releaseObject.transform.position.z+5f);
        float ignoreDistance = Vector3.Distance(cam.transform.position, ignoreVector);
        for(int i = 0; i < hits.Length;i++){
            // if (hits[i].distance > Vector3.Distance(cam.transform.position, releaseObject.transform.position)){
            //     hit = hits[i];
            //     break;
            // }
            if (hits[i].distance > ignoreDistance){
                hit = hits[i];
                break;
            }
        }
        
        if(hit.collider.tag == "Player" && Vector3.Distance(hit.point, cam.transform.position) <=4f){

            Vector3 unitVec = cam.transform.forward.normalized;
            return unitVec*500f;
        }
        return hit.point;
    }
    public void DrawRay()
    {
        GameObject cam = Camera.main.gameObject;
        Debug.DrawRay(cam.transform.position, GetMouseDirection().normalized*100f , Color.red);
        
        Debug.DrawRay(releaseObject.transform.position, releaseObject.transform.forward.normalized*100f , Color.blue);
    }
    // Parent ray cast function , rifles, launchers and pistol types can use this
    // 
    protected RaycastHit GetRaycastHit(){
        Vector3 direction = GetMouseDirection();
        RaycastHit hit;
        if (Physics.Raycast(releaseObject.transform.position, direction, out hit, Mathf.Infinity)){
            Debug.Log(hit.collider.gameObject);
        }
        return hit;
    }
    // SUB_CLASS HELPERS
    public bool IsFireable(){
        // constrain rate of fire
        // currentAmmoInMag > 0
        if (lastFiredTime + FireRate < Time.time && currentLoadedAmmo > 0 && IsReloaded() ) {
            return true;
        }else {
            return false;
        }
        
    }
    protected bool IsReloadable(){
        if (currentLoadedAmmo < magazineSize && currentNonLoadedAmmo > 0){
            return true;
        }else{
            return false;
        }
    }
    public bool IsReloaded(){
        if (isReloading)
        {
            if (lastReloadTime + reloadTime < Time.time){
                Reload();
                isReloading  = false;
                return true;
            }else {
                return false;
            }
        }else{
            if (currentLoadedAmmo == 0 && currentNonLoadedAmmo > 0){
                InitiateReloading();
            }
            return true;
        }
    }
    
// GETTERS AND SETTERS

    public int getCurrentMag(){
        return currentLoadedAmmo;
    }
    public int getUnloadedAmmo(){
        return currentNonLoadedAmmo;
    }
    public string getName(){
        return Name;
    }
    public string getStatus(){
        if (isReloading){
            return "RELOADING";
        } else {
            return "READY TO FIRE";
        }
    }
    public void addAmmo(int ammo){
        // maxAmmo, currentNonLoadedAmmo
        if(ammo+currentNonLoadedAmmo+currentLoadedAmmo > maxAmmo){
            currentNonLoadedAmmo = maxAmmo - currentLoadedAmmo;
        }else{
            currentNonLoadedAmmo += ammo;
        }
    }
    // MP
    public virtual GameObject FireMP(){
        Debug.Log("FIRE MP");
        // Fire();
        if (IsFireable()){
        lastFiredTime = Time.time;
        currentLoadedAmmo -=1;
        }
        return null;
    }
}
