using UnityEngine;
using System.Collections;
using System;

public class AttackEnemy : Leaf
{
    public override NodeStatus OnBehave(BehaviourState state)
    {
        Context context = (Context)state;

        if (context.enemy == null)
            return NodeStatus.FAILURE;

        context.me.DealDamage(context.enemy);
        context.me.LinkAttackWithAnimator();
        

        // TODO - perhaps should test success of the actual attack and return failure if we missed

        return NodeStatus.SUCCESS;
    }

    public override void OnReset()
    {
    }
}
