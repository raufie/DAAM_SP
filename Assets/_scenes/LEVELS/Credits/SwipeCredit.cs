using UnityEngine;

public class SwipeCredit : CreditBase
{
    public float period = 5f;
    public float swipeSpeed = 0.05f;
    public Vector3 p;
    private bool isLaunched;
    private bool isSwiped;
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
        if(isLaunched && !isSwiped){
            
            GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(GetComponent<RectTransform>().anchoredPosition3D, p,swipeSpeed*Time.deltaTime);
        }
        if(Mathf.Abs(GetComponent<RectTransform>().anchoredPosition3D.y - p.y)<0.5f && !isSwiped){
            
            isSwiped = true;
            ViewingTime = Time.time;
        }
        if (isSwiped && !isViewed){
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
