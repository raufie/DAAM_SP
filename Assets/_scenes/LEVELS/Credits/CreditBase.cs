using UnityEngine;

public class CreditBase : MonoBehaviour
{
    public virtual void Initialize(){

    }
    public void EndCredit(){
        GameObject.FindGameObjectWithTag("CreditsManager").GetComponent<CreditsManager>().NextCredit();
        Destroy(gameObject);
    }
}
