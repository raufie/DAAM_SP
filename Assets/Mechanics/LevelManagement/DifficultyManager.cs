using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    
    // we must have a list of all the enmies
    struct DifficultyProfile {
        public float Speed;
        public int DamagePoints;
    }
    public enum DifficultyLevel{
        LOW, MEDIUM, HIGH
    }
    DifficultyProfile [] DroneDifficulties = new DifficultyProfile[3]{
        NewProfile(0.2f, 3),
        NewProfile(0.5f, 5),
        NewProfile(0.85f, 10)
    };
    DifficultyProfile [] SpiderDifficulties = new DifficultyProfile[3]{
        NewProfile(0.25f, 5),
        NewProfile(0.5f, 10),
        NewProfile(1f, 15)
    };
    DifficultyProfile [] TripodDifficulties = new DifficultyProfile[3]{
        NewProfile(0.4f, 3),
        NewProfile(0.55f, 7),
        NewProfile(0.85f, 10)
    };
    DifficultyProfile [] TankDifficulties = new DifficultyProfile[3]{
        NewProfile(0.20f, 10),
        NewProfile(0.35f, 15),
        NewProfile(0.85f, 30)
    };
    // Difficulty

    // public assignments
    private GameObject [] Enemies;
    public DifficultyLevel difficultyLevel;

    // Start is called before the first frame update
    void Start()
    {
        Enemies = GetComponent<LevelManager>().Enemies;
        // SetDifficultyProfiles();
    }

    public void SetDifficultyProfiles(int diff = -1){
        Enemies = GetComponent<LevelManager>().Enemies;
        if (diff >= 0 && diff <= 2){
            difficultyLevel =(DifficultyLevel)(diff);
        }else {
            return;
        }

        for (int i = 0; i < Enemies.Length; i++){
            GameObject enemyObject = Enemies[i];
            if(enemyObject == null){
                continue;
            }
            if((enemyObject.GetComponent("DroneController") as DroneController) != null){
                // set drone difficulty here
                enemyObject.GetComponent<DroneController>().speed = DroneDifficulties[(int)difficultyLevel].Speed;
                enemyObject.GetComponent<DroneController>().DamagePoints = DroneDifficulties[(int)difficultyLevel].DamagePoints;
            }
            else if((enemyObject.GetComponent("TripodController") as TripodController) != null){
                // set drone difficulty here
                enemyObject.GetComponent<TripodController>().speed = TripodDifficulties[(int)difficultyLevel].Speed;
                enemyObject.GetComponent<TripodController>().DamagePoints = TripodDifficulties[(int)difficultyLevel].DamagePoints;
            }
            else if((enemyObject.GetComponent("TankController") as TankController) != null){
                // set drone difficulty here
                enemyObject.GetComponent<TankController>().speed = TankDifficulties[(int)difficultyLevel].Speed;
                enemyObject.GetComponent<TankController>().DamagePoints = TankDifficulties[(int)difficultyLevel].DamagePoints;
            }
            else if((enemyObject.GetComponent("SpiderController") as SpiderController) != null){
                // set drone difficulty here
                enemyObject.GetComponent<SpiderController>().speed = SpiderDifficulties[(int)difficultyLevel].Speed;
                enemyObject.GetComponent<SpiderController>().DamagePoints = SpiderDifficulties[(int)difficultyLevel].DamagePoints;
            }
        }

    }
    private static DifficultyProfile NewProfile(float speed, int damagePoints){
        DifficultyProfile profile;
        profile.Speed = speed;
        profile.DamagePoints = damagePoints;
        return profile;
    }
}
