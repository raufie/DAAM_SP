using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponBase
{
    // Start is called before the first frame update
    public MachineGun(
        string _Name, 
        float _FireRate, 
        GameObject _releaseObject,
        int _maxAmmo,
        int _magazineSize,
        int _currentLoadedAmmo,
        int _currentNonLoadedAmmo,
        int _damagePoints,
        float _reloadTime
        ):
        base(
        _Name, 
        _FireRate,
        _releaseObject,
        _maxAmmo,
        _magazineSize,
        _currentLoadedAmmo,
        _currentNonLoadedAmmo,
        _damagePoints,
        _reloadTime
        )
    {
        
    }
    public override void Fire(){
        
        if (IsFireable()){
        RaycastHit hit = GetRaycastHit();
            if(hit.collider != null){
                if (hit.collider.tag == "Enemy") {
                    hit.collider.gameObject.GetComponent<EnemyBase>().takeDamage(damagePoints);
                }
            }
        lastFiredTime = Time.time;

        }
      
    }
}
