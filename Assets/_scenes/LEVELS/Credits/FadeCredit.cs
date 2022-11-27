using UnityEngine;

public class FadeCredit : CreditBase
{
    public float period = 5f;
    public float FadeSpeed = 0.01f;
    private bool isLaunched;
    private bool isFadedIn;
    private float ViewingTime;
    private bool isViewed;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }
    void FixedUpdate(){
        
        // swipe to target
        // fade after period
        if(isLaunched && !isFadedIn){
            // FADE IN
            GetComponent<CanvasGroup>().alpha += 0.01f;
        }
        if(GetComponent<CanvasGroup>().alpha == 1 && !isFadedIn){
            
            isFadedIn = true;
            ViewingTime = Time.time;
        }
        if (isFadedIn && !isViewed){
            if(Time.time > ViewingTime + period){
                isViewed = true;
            }
        }
        if(isViewed){
            GetComponent<CanvasGroup>().alpha -= 0.01f;
            if(GetComponent<CanvasGroup>().alpha == 0){
                EndCredit();
            }
        }
        Debug.Log(Time.deltaTime);

    }
    public override void Initialize(){
        isLaunched = true;
    }
}
