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
        setTrack("lvl1wp1");
    }

    // NOT ADVISED FOR A LINEAR SOUNDTRACK experience
    public void setTrack(string trackName){
        currentTrack = trackName;
        updatePlayback();
    }
    public void updatePlayback(){
        audioManager.startBGM(currentTrack);
    }
    public void next(){

    }
    
}
