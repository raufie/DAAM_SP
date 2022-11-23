using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Prompt : MonoBehaviour
{
    public Button CloseButton;
    public Button BG_MODAL;
    // Start is called before the first frame update
    void Start()
    {
        CloseButton.onClick.AddListener(()=>{
            gameObject.SetActive(false);
        });
        try{
            BG_MODAL.onClick.AddListener(()=>{
                gameObject.SetActive(false);
            });
        }catch {
            
        }
    }

}
