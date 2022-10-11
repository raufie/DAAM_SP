using UnityEngine;

public class AreaMilestone : MilestoneBase
{
    public float Radius = 5f;


    public override bool CheckFinished(){
        if (Vector3.Distance(Player.transform.position, transform.position) < Radius){
            return true;
        }
        return false;
    }
    void OnDestroy(){
        Destroy(VisualsManager);
    }
}
