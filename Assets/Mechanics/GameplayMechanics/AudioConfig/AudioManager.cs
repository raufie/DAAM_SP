using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public Dictionary<string, GameObject> SFXObjects = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> MusicObjects = new Dictionary<string, GameObject>();
    
// MUSIC MANAGEMENT IS DIFFERENTTT
    // public string [,] bruh = new string [];
    public string [] sfxAudioNames ;
    public GameObject [] sfxAudioSources ;
    public string [] musicAudioNames ;
    public GameObject [] musicAudioSources ;

// Settings
    public float sfxVolume =1.0f;
    public float bgmVolume =1.0f;
    public float masterVolume =1.0f;

    public GameObject currentMusicTrack;
    private bool isBGMPlaying = true;
    // CONStrct

// MUSIC MANAGEMENET
    void Start(){
        // load audio prefs
        // populate dictionary
        for(int i =0; i < sfxAudioNames.Length; i++){
            SFXObjects.Add(sfxAudioNames[i], sfxAudioSources[i]);
        }
        for(int i =0; i < musicAudioNames.Length; i++){
            MusicObjects.Add(musicAudioNames[i], musicAudioSources[i]);
        }
        
        // ADD MUSIC MANAGER
        MusicManager musicManager = gameObject.AddComponent<MusicManager>();
        musicManager.audioManager = gameObject.GetComponent<AudioManager>();
        load();
    }
    void Update(){
        if(Time.timeScale == 0f)
        {
            try {
                currentMusicTrack.GetComponent<AudioObject>().audioSource.Pause();
            }catch {
                Debug.Log("Current music track is null");
            }
            isBGMPlaying = false;
        }
        else
        {
            if(!isBGMPlaying){
                currentMusicTrack.GetComponent<AudioObject>().audioSource.Play();
                isBGMPlaying = true;
            }
        }
    }
    void FixedUpdate(){
        // Debug.Log("WALKING", isWalking);
        // Debug.Log("Running", isRunning);
        // WALK/RUN
    }
    public GameObject fireSFXEvent(string type, Vector3 position){
        GameObject audioObject = null;
        try{
        SFXObjects[type].GetComponent<AudioObject>().audioSource.volume = sfxVolume*masterVolume;
        audioObject = Instantiate(SFXObjects[type], position, Quaternion.identity);
        }catch{
            Debug.Log("Error playing audio");
        }
        return audioObject;
    }
    public GameObject fireTrackingSFXEvent(string type, GameObject parentObject){
        GameObject audioObject = null;
        try{
        SFXObjects[type].GetComponent<AudioObject>().audioSource.volume = sfxVolume*masterVolume;
        audioObject = Instantiate(SFXObjects[type], parentObject.transform.position, Quaternion.identity);
        audioObject.transform.parent = gameObject.transform;
        }catch{
            Debug.Log("Error playing audio");
        }
        return audioObject;
    }
    public void startBGM(string type){

        
        try{
            MusicObjects[type].GetComponent<AudioObject>().audioSource.volume = bgmVolume*masterVolume;
            Destroy(currentMusicTrack.gameObject);
            currentMusicTrack = Instantiate(MusicObjects[type], transform.position, Quaternion.identity);
        }catch{
            Debug.Log("error destroying game object");
        }
        

    }
    public void load(){
        Debug.Log("loading audio");
        masterVolume = (float)PlayerPrefs.GetFloat("masterVolume");
        sfxVolume = (float)PlayerPrefs.GetFloat("sfxVolume");
        bgmVolume = (float)PlayerPrefs.GetFloat("bgmVolume");
        for(int i = 0; i < sfxAudioSources.Length;i++){
            try{
                sfxAudioSources[i].GetComponent<AudioObject>().audioSource.volume = sfxVolume*masterVolume;
            }catch{
                    
            }
        }
        try{
        currentMusicTrack.GetComponent<AudioObject>().audioSource.volume = bgmVolume*masterVolume;
        }catch{
            
        }
    }
    public void save(float _masterVolume, float _sfxVolume, float _bgmVolume){
        PlayerPrefs.SetFloat("masterVolume", _masterVolume);
        PlayerPrefs.SetFloat("sfxVolume", _sfxVolume);
        PlayerPrefs.SetFloat("bgmVolume", _bgmVolume);
        PlayerPrefs.Save();
        load();
    }
}
