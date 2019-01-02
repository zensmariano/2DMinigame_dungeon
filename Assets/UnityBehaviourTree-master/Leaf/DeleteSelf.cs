using UnityEngine;
using System.Collections;
using System;

public class DeleteSelf : Leaf
{
    public override NodeStatus OnBehave(BehaviourState state)
    {
        Context context = (Context)state;
        context.me.DeleteSelf();
        return NodeStatus.SUCCESS;
    }

    public override void OnReset()
    {
    }
}