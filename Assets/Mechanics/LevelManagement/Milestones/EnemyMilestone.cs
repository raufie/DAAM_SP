using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMilestone : MilestoneBase
{
    public GameObject Enemy;
    public override bool CheckFinished(){
        if (Enemy.GetComponent<EnemyBase>().healthPoints <=0){
            return true;
        }
        return false;
    }
}
