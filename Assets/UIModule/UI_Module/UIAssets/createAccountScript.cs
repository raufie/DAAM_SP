using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class createAccountScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cancelBtn;
    public GameObject createBtn;
    void Start()
    {
        cancelBtn.GetComponent<Button>().onClick.AddListener(closeScreen);
        createBtn.GetComponent<Button>().onClick.AddListener(closeScreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void closeScreen(){
        gameObject.SetActive(false);
    }
}
