using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    // Start is called before the first frame update
    public int Health;
    [Header("ALL BELOW IS DEBUG")]
    public Material deadMaterial;

    void Update(){
    }
    public void takeDamage(int damage)
    {
        if (Health <= 0)
        {
            // Destroy(gameObject);
            
            Debug.Log("Player is dead");
            
        }
        else if(Health - damage <0){
            Health = 0;
        }
        else{
            Health -= damage;
        }
        float color = ((((float)Health/100f))*255f);
        Debug.Log(color);
        // GetComponent<Renderer>().material.color = new Color(255,color,243);
    }
    public void takeHealth(int healthToAdd){
        // add health but clamp to 100
        int newHealth = healthToAdd + Health;
        Health = Mathf.Clamp(newHealth, 0, 100);
    
        Debug.Log(Health);
    }
    private void OnGUI() {
        GUI.backgroundColor = Color.yellow;
        GUI.Button(new Rect(20, 10, 150, 20)," Player Health: " + Health );
    }
}
