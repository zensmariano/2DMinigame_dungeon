using UnityEngine;
using System.Collections;
using System;

public class SetRandomDestination : Leaf {

    public override NodeStatus OnBehave(BehaviourState state)
    {
        Context context = (Context)state;
        context.moveTarget = (Vector2)context.me.transform.position + (UnityEngine.Random.insideUnitCircle * 4);
        return NodeStatus.SUCCESS;
    }

    public override void OnReset()
    {
    }

}
