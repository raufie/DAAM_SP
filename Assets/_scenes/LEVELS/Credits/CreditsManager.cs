using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CreditsManager : MonoBehaviour
{
    public CreditBase [] Credits;
    public Button btn;
    int CurrentCredit;

    void Start(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        LaunchCredits();
        btn.onClick.AddListener(()=>{
            SceneManager.LoadScene(0);
        });
    }
    void LaunchCredits(){
        Credits[0].GetComponent<CreditBase>().Initialize();
    }
    public void NextCredit(){
        // Initialize NEXT CREDIT
        CurrentCredit++;
        if(CurrentCredit < Credits.Length ){
            Credits[CurrentCredit].GetComponent<CreditBase>().Initialize();
        }
    }
}
