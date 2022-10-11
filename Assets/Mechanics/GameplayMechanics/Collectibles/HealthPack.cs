using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    
    public int healthPoints = 25;
    public float rotationSpeed = 10.5f;
    void Update(){
        transform.Rotate(0f,rotationSpeed*Time.deltaTime, 0f);
    }
    private void OnTriggerEnter(Collider other){
        Debug.Log(other.tag);
        if(other.tag == "Player"){
            Debug.Log(other.gameObject);
            // FindParentWithTag(other.gameObject, "Player").GetComponent<PlayerObject>().takeHealth(healthPoints);
            // Debug.Log(FindParentWithTag(other.gameObject, "Player"));
            other.gameObject.GetComponent<PlayerObject>().takeHealth(healthPoints);
            Destroy(gameObject );
        }
    }
    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
        if (t.parent.tag == tag)
        {
            return t.parent.gameObject;
        }
        t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }

}
