using UnityEngine;
using System.Collections;
using System;

public class StopMoving : Leaf
{
    public override NodeStatus OnBehave(BehaviourState state)
    {
        Context context = (Context)state;
        context.moveTarget= context.me.transform.position;
        return NodeStatus.SUCCESS;
    }

    public override void OnReset()
    {
    }
}
