using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceManager : MonoBehaviour
{
    public static GameObject InstantiateStatic(GameObject obj, Vector3 position, Quaternion rotation){
        return Instantiate(obj, position, rotation);
    }
}
