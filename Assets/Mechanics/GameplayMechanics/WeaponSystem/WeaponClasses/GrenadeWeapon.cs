using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeWeapon : WeaponBase
{
    private GameObject grenadePrefab;
    private float grenadeTimer;
    private float explosionRadius;
    // explosions
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public GrenadeWeapon(
        string _Name, 
        float _FireRate, 
        GameObject _releaseObject,
        int _maxAmmo,
        int _magazineSize,
        int _currentLoadedAmmo,
        int _currentNonLoadedAmmo,
        int _damagePoints,
        float _reloadTime,
        GameObject _grenadePrefab,
        float _grenadeTimer,
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
        grenadePrefab = _grenadePrefab;
        grenadeTimer = _grenadeTimer;
        explosionRadius = _explosionRadius;

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Fire(){

        if (IsFireable()){
        Debug.Log("Instantiating");
        Vector3 direction = GetMouseDirection();
        GameObject grenadeInstance = InstanceManager.InstantiateStatic(grenadePrefab,releaseObject.transform.position, Quaternion.identity);
        grenadeInstance.GetComponent<GrenadeProjectile>().timer = grenadeTimer;
        grenadeInstance.GetComponent<GrenadeProjectile>().radius = explosionRadius;
        grenadeInstance.GetComponent<GrenadeProjectile>().damagePoints = damagePoints;
        grenadeInstance.GetComponent<GrenadeProjectile>().Launch(direction);
        
        }
        base.Fire();
    }
    
}
