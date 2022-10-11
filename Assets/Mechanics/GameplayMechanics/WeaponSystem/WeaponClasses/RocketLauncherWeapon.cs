using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherWeapon : WeaponBase
{
    private GameObject rocketPrefab;
    private float explosionTimer;
    private float explosionRadius;

    public RocketLauncherWeapon(
        string _Name, 
        float _FireRate, 
        GameObject _releaseObject,
        int _maxAmmo,
        int _magazineSize,
        int _currentLoadedAmmo,
        int _currentNonLoadedAmmo,
        int _damagePoints,
        float _reloadTime,
        GameObject _rocketPrefab,
        float _explosionTimer,
        float _explosionRadius
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
        rocketPrefab = _rocketPrefab;
        explosionTimer = _explosionTimer;
        explosionRadius = _explosionRadius;
    }
    public override void Fire(){

        if (IsFireable()){
  
        Vector3 direction = GetMouseDirection();
        GameObject rocketInstance = InstanceManager.InstantiateStatic(rocketPrefab,releaseObject.transform.position, getRotationToTarget());
        rocketInstance.GetComponent<RocketProjectile>().timer = explosionTimer;
        rocketInstance.GetComponent<RocketProjectile>().radius = explosionRadius;
        rocketInstance.GetComponent<RocketProjectile>().damagePoints = damagePoints;
        rocketInstance.GetComponent<RocketProjectile>().Launch(direction);
        
        }
        base.Fire();
    }

    private Quaternion getRotationToTarget(){
        RaycastHit hit = GetRaycastHit();

        var lookPos = GetMouseDirection();
        float damping = 1.0f;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        rotation *= Quaternion.Euler(0, 90, 0); 
        return rotation;

    }
    
}
