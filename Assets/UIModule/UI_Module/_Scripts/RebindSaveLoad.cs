using UnityEngine;
using UnityEngine.InputSystem;
public class RebindSaveLoad : MonoBehaviour
{
    [SerializeField]
    public PlayerInput actions;
    
    void Awake(){
        actions = new PlayerInput();
    }
    
    
}
