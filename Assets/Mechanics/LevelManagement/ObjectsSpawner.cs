using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
    public GameObject []Enemies;
    public float radius;
    public GameObject player;
    public bool hasSpawned = false;
        // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        DespawnAll();
    }

    void Update(){
        if (Vector3.Distance(transform.position, player.transform.position) < radius && hasSpawned == false){
            hasSpawned = true;
            SpawnAll();
        }
    }
    void SpawnAll(){
        for(int i =0; i < Enemies.Length;i++){
            if(Enemies[i] != null && !Enemies[i].GetComponent<Spawnable>().IsAlive()){
                Enemies[i].transform.parent.gameObject.SetActive(true);
                Enemies[i].SetActive(true);
                Enemies[i].GetComponent<Spawnable>().SetAlive();
            }
           
        }
    }
    void DespawnAll(){
        for(int i =0; i < Enemies.Length;i++){
            if(Enemies[i] != null && !Enemies[i].GetComponent<Spawnable>().IsAlive()){
                Enemies[i].transform.parent.gameObject.SetActive(false);
                Enemies[i].GetComponent<Spawnable>().SetInactive();
            }
           
        }
    }
    
}
