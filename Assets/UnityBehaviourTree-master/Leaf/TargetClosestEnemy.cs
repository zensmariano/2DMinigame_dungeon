using UnityEngine;
using System.Collections;
using System;

public class TargetClosestEnemy : Leaf
{
    float distanceThreshold;
    public TargetClosestEnemy(float threshold)
    {
        distanceThreshold = threshold;
    }

    public override NodeStatus OnBehave(BehaviourState state)
    {
        Context context = (Context)state;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
       
        
        Array.Sort(players, delegate (GameObject p1, GameObject p2)
        {
            float d1 = context.me.DistanceTo(p1.transform.position);
            float d2 = context.me.DistanceTo(p2.transform.position);

            return d1.CompareTo(d2);
        });

        foreach(GameObject l in players)
        {
            if(l != context.me.gameObject)
            {
                if(context.me.DistanceTo(l.transform.position) > distanceThreshold)
                {
                    context.enemy = null;
                    return NodeStatus.FAILURE;
                } 

                context.enemy = l.GetComponent<PlayerController_2D>();
                return NodeStatus.SUCCESS;
            }
        }
        context.enemy = null;
        return NodeStatus.FAILURE;
    }

    public override void OnReset()
    {
    }
}
