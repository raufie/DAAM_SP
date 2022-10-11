using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RocketProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    private float timeStarted;
    public float timer;
    public int damagePoints;
    public float radius;
    public float speed = 75f;
    private float armedTimer;//so that it doesn't collide and destroy the player
    private AudioManager audioManager;
    private GameObject rocketPropulsionAudioObject;
    void Start()
    {
        audioManager = GameObject.FindGameObjectsWithTag("AudioManager")[0].GetComponent<AudioManager>();

        rocketPropulsionAudioObject = audioManager.fireTrackingSFXEvent("RocketExhaust", gameObject);
        armedTimer = 0.05f + Time.time;
        // Debug.Log(Vector3.Distance(transform.position, new Vector3(transform.position.x+5f, transform.position.y+5f, transform.position.z+5f)));
        Debug.Log(transform.position);
    }


    // Update is called once per frame
    void Update()
    {
        if ( Time.time > timeStarted + timer){
            Explode(damagePoints, radius);
        }
             if (Time.time> armedTimer ){
            gameObject.GetComponent<BoxCollider>().enabled =true;
        }
    }

    // launch
    public void Launch(Vector3 direction){
        timeStarted = Time.time;
        gameObject.GetComponent<Rigidbody>().velocity = direction*speed;
    }


    // explode
    public void Explode(int damagePoints, float explosionRadius){
        RaycastHit[] hits;

        hits = Physics.SphereCastAll(transform.position, explosionRadius, transform.forward, explosionRadius);
        
        foreach (var hit in hits)
        {
            try {

                // things that can take damage, take damage
                if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Player")
                {

                    if (hit.collider.gameObject.tag == "Enemy" ){
                    
                    hit.collider.gameObject.GetComponent<EnemyBase>().takeDamage(calculateHitPoints(hit));
                    
                    } else {
                        
                        hit.collider.gameObject.GetComponent<PlayerObject>().takeDamage(calculateHitPoints(hit));
                    }
                }
                // things that have rigidbody get force
                if (hit.collider.gameObject.GetComponent<Rigidbody>() != null){
                    // add force
                    // direction opposite (rocket - hit obj)
                    Vector3 forceDirection = (transform.position - hit.collider.gameObject.transform.position)*-1f;
                    hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(forceDirection*calculateHitPoints(hit)*7.5f);

                }
            } 
            catch(Exception e){
                print("errr");
            }
        }
        Destroy(gameObject);
    }
    // UTILITIES
    private void OnCollisionEnter(Collision other) {
        Explode(damagePoints, radius);
    }
    private int calculateHitPoints(RaycastHit hit){
        float maxDistance = Vector3.Distance(transform.position, new Vector3(transform.position.x+radius, transform.position.y+radius, transform.position.z+radius));
        float hitDistance = Vector3.Distance(transform.position,hit.collider.gameObject.transform.position);
        int damage;
        // Debug.Log(hitDistance+" "+ maxDistance);
        if (hitDistance <= maxDistance){
            damage = (int)((1.0f-(hitDistance/maxDistance)) * damagePoints);
            // Debug.Log("giving damage in range"+ damage);
        }
        else{
            
            damage = 0;
        }
        return damage;
    }   
    void OnDestroy(){
        Destroy(rocketPropulsionAudioObject);
        audioManager.fireSFXEvent("explosion",transform.position);
    }

}
