using UnityEngine;
using System.Collections;
using System;

public class Move : Leaf
{
    public override NodeStatus OnBehave(BehaviourState state)
    {
        Context context = (Context)state;

        if (!context.moveTarget.HasValue)
            return NodeStatus.FAILURE;
            
        if (AtDestination(context) || IsStopped(context)){
            return NodeStatus.SUCCESS;
        }

        context.me.transform.position += ((Vector3)context.moveTarget - context.me.transform.position) * Time.deltaTime;
        context.me.LinkMovementWithAnimator((Vector3)context.moveTarget);

        return NodeStatus.RUNNING;
    }

    bool AtDestination(Context context)
    {
        if (context.me.DistanceTo(context.moveTarget.Value) < 2f)
        {
            return true;
        }
        return false;
    }

    bool IsStopped(Context context){

        if(context.me.transform.position == context.me.GetLastPosition() && context.me.HasWaitedBeforeRepathing())
        return true;

        return false;
    }

    public override void OnReset()
    {
    }
}