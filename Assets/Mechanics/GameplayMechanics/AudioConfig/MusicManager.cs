using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioManager audioManager;
    public string currentTrack;
    public int currentIndex;
    // Areas of importance
    public Area [] areasOfImportance;
    // VIP THAT areasOfImportance.Length == musicTracks
    // level music stack
    void Start(){
        try{
        int currentLevel = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<MilestonesManager>().CURRENT_LEVEL;
        setTrack("lvl"+currentLevel);
        }catch {
        setTrack("lvl1");
        }
        // get level , set based on that
    }

    // NOT ADVISED FOR A LINEAR SOUNDTRACK experience
    public void setTrack(string trackName){
        currentTrack = trackName;
        updatePlayback();
    }
    public void updatePlayback(){
        Debug.Log("current track:"+currentTrack);
        audioManager.startBGM(currentTrack);
    }
    public void next(){

    }
    
}
