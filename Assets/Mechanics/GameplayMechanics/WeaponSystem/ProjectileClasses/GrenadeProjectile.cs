using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    
    private float timeStarted;
    public float timer;
    public int damagePoints;
    public float radius;
    private AudioManager audioManager;
    public GameObject ExplosionPrefab;
    void Start()
    {
    audioManager = GameObject.FindGameObjectsWithTag("AudioManager")[0].GetComponent<AudioManager>();
    
    }

    // Update is called once per frame
    void Update()
    {
        if ( Time.time > timeStarted + timer){
            Explode(damagePoints, radius);
        }
   

    }

    // launch
    public void Launch(Vector3 direction){
        timeStarted = Time.time;
        gameObject.GetComponent<Rigidbody>().velocity = direction*20;
    }

    // explode
    public void Explode(int damagePoints, float grenadeExplosionRadius){
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, grenadeExplosionRadius, transform.forward, grenadeExplosionRadius);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Player")
            {
                if (hit.collider.gameObject.tag == "Enemy" ){
                hit.collider.gameObject.GetComponent<EnemyBase>().takeDamage(calculateHitPoints(hit));
                }else {

                    hit.collider.gameObject.GetComponent<PlayerObject>().takeDamage(calculateHitPoints(hit));
                   
                }
            }else {
                 if (hit.collider.gameObject.GetComponent<Rigidbody>() != null){
                    // add force
                    // direction opposite (rocket - hit obj)
                    Vector3 forceDirection = (transform.position - hit.collider.gameObject.transform.position)*-1f;
                    hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(forceDirection*calculateHitPoints(hit)*5f);

                }
            }
        }
        Destroy(gameObject);
    }
    private int calculateHitPoints(RaycastHit hit){
        float maxDistance = Vector3.Distance(transform.position, new Vector3(transform.position.x+radius, transform.position.y+radius, transform.position.z+radius));
        float hitDistance = Vector3.Distance(transform.position,hit.collider.gameObject.transform.position);
        int damage;
        Debug.Log(hitDistance+" "+ maxDistance);
        if (hitDistance <= maxDistance){
            damage = (int)((1.0f-(hitDistance/maxDistance)) * damagePoints);
            Debug.Log("giving damage in range"+ damage);
        }
        else{
            
            damage = 0;
        }
        return damage;
    }   
    void OnDestroy(){
        audioManager.fireSFXEvent("explosion",transform.position);
    }
}
