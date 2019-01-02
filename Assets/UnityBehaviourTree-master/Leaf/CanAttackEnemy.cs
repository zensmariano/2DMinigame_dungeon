using UnityEngine;
using System.Collections;
using System;

public class CanAttackEnemy : Leaf
{
    public override NodeStatus OnBehave(BehaviourState state)
    {
        Context context = (Context)state;

        if (context.enemy == null)
            return NodeStatus.FAILURE;

        if(context.me.DistanceTo(context.enemy.transform.position) <= 2f){
            return NodeStatus.SUCCESS;
        }
        context.me.GetComponent<Animator>().SetBool("IsAttacking",false);
        return NodeStatus.FAILURE;
    }

    public override void OnReset()
    {
    }
}
