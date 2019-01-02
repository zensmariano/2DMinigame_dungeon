using UnityEngine;
using System.Collections;

public class Context : BehaviourState
{
    public EnemyController me ;

    public Context(EnemyController thisEnemy){
        me = thisEnemy;
    }

    [HideInInspector]
        public PlayerController_2D enemy = null;

    [HideInInspector]
        public Vector3? moveTarget = null;
}
