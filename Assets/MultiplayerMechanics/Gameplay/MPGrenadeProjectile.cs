using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MPGrenadeProjectile : NetworkBehaviour
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
            if (hit.collider.gameObject.tag == "Player")
            {
                GameObject playerObject = NetworkClient.localPlayer.gameObject;
                playerObject.GetComponent<NetworkPlayerObject>().CmdDamage(hit.collider.gameObject, 25);

                    // hit.collider.gameObject.GetComponent<PlayerObject>().takeDamage(calculateHitPoints(hit));
                hit.collider.gameObject.GetComponent<NetworkTelemetaryObject>().KillPlayer();
                    playerObject.GetComponent<NetworkPlayerObject>().CmdAddKill(playerObject.GetComponent<NetworkPlayerObject>().Team, playerObject.GetComponent<NetworkPlayerObject>().ID, playerObject, hit.collider.gameObject);
                    DataManager.AddKill();
                
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
