using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // Start is called before the first frame update
    public int healthPoints;
    public void takeDamage(int _damageTaken){
        if(_damageTaken >= healthPoints){
            // DO Whatever you want when the enemy is destroyed
            Destroy(gameObject);
        }else {
            healthPoints -= _damageTaken;
        }
    }
    void OnEnable(){
     
    }
}
