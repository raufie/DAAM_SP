using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    public AudioSource audioSource;
    public float timeToKill;
    public bool isSfx = true;
    void Start(){
 
        timeToKill = audioSource.clip.length + Time.time;
        audioSource.Play();
    }
    void Update(){
        if(isSfx && Time.time > timeToKill){
            Destroy(gameObject);
        }
    }
}
