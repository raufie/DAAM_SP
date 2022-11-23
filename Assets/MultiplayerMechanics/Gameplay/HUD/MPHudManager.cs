using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Mirror;
public class MPHudManager : MonoBehaviour
{
    // HEALTH
    public RectTransform HealthBarWidth;
    public NetworkTelemetaryObject playerObject;
    // MILESTONES AND INFO
    
    public bool IsViewingInfo = false;
    public GameObject PlayersTable;
    public TMP_Text info_label;
    public TMP_Text info_desc;

    public Sprite BlastarSprite;
    public Sprite GrenadeSprite;
    public Sprite RifleSprite;
    public Sprite RocketSprite;

    public Image CurrentWeaponImage;
    public TMP_Text Ammo;
    // score
    public TMP_Text Score1;
    public TMP_Text Score2;
    // Time
    public TMP_Text TimeText;

    public GameObject MapDisplay;
    // INTERNAL
    private InputMaster controls;
    
    

    void Awake(){
        controls = new InputMaster();
        Time.timeScale = 1f;
        RefreshInput();
        // playerObject = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerObject>();
        // if(isLocalPlayer){
        // GameObject localPlayer = NetworkClient.localPlayer.gameObject;
        // }
        // playerObject must be localPlayer
    }    
    void OnEnable(){
        LoadSettings();
    }
    private void LoadSettings(){
        
           // LOADING
        var rebinds = PlayerPrefs.GetString("rebinds");
        if(!string.IsNullOrEmpty(rebinds)){
            controls.LoadBindingOverridesFromJson(rebinds);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
         // THE BELOW IS SUBJECT TO CHANGE
        
        if(controls.Player.Info.ReadValue<float>()>=0.5f){
            IsViewingInfo = true;
            PlayersTable.SetActive(true);
        }else{
            IsViewingInfo = false;
            PlayersTable.SetActive(false);
        }
 

        if(playerObject == null){
            Ammo.text = "-\n-";
            return;
        }
        // health.text = ""+ playerObject.Health;
        HealthBarWidth.sizeDelta = new Vector2(playerObject.Health, 100);
        
        if((int)playerObject.gameObject.GetComponent<MPWeaponsManager>().currentWeapon==1){
            CurrentWeaponImage.sprite = BlastarSprite;
        }else if ((int)playerObject.gameObject.GetComponent<MPWeaponsManager>().currentWeapon==2){
            CurrentWeaponImage.sprite = RifleSprite;
        }else if ((int)playerObject.gameObject.GetComponent<MPWeaponsManager>().currentWeapon==3){
            CurrentWeaponImage.sprite = RocketSprite;
        }else if ((int)playerObject.gameObject.GetComponent<MPWeaponsManager>().currentWeapon==4){
            CurrentWeaponImage.sprite = GrenadeSprite;
        }

        Ammo.text = playerObject.gameObject.GetComponent<MPWeaponsManager>().GetCurrentWeapon().currentLoadedAmmo + "\n"+ playerObject.gameObject.GetComponent<MPWeaponsManager>().GetCurrentWeapon().currentNonLoadedAmmo;

       

        // MilestoneSprite.GetComponent< = screenPos;
    }

    private void RefreshInput(){
        
        
 
        if (GameObject.FindGameObjectsWithTag("InputManagement").Length == 0){
            controls = new InputMaster();
        }
        else{
            
            controls =  GameObject.FindGameObjectsWithTag("InputManagement")[0].GetComponent<MPInputManagement>().GetActions();
        }
        if(controls == null){
            controls = new InputMaster();
        }
        controls.Enable();
        LoadSettings();
    }
    public void UpdateScore(int score1, int score2){
        Score1.text = ""+score1;
        Score2.text = ""+score2;
    }
    public void UpdateTime(float t){
        int h = (int) (t/60f);
        int s = (int)(t%60f);
        TimeText.text = h+":"+s;
    }
    public void UpdateInfoEntry(string username, int kills, int deaths, int index){
        PlayersTable.GetComponent<TableUpdateManager>().AddEntryAtIndex(username, ""+kills, ""+deaths, index);
    }
    public void UpdatePlayerObject(){
        playerObject = NetworkClient.localPlayer.gameObject.GetComponent<NetworkTelemetaryObject>();
    }
}
