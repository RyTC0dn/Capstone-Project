using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Patrol", story: "[NPC] navigates to [Waypoints] at [MoveSpeed]", category: "Action", id: "41731dffaa6829e9adb33a70e04cddc4")]
public partial class PatrolEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> NPC;
    [SerializeReference] public BlackboardVariable<List<Vector2>> Waypoints;
    [SerializeReference] public BlackboardVariable<float> MoveSpeed;

    private int waypointIndex;

    protected override Status OnStart()
    {
        waypointIndex = 0;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        NPC.Value.transform.position = Vector2.MoveTowards(NPC.Value.transform.position, 
            Waypoints.Value[waypointIndex], MoveSpeed * Time.deltaTime);

        if(Vector2.Distance(NPC.Value.transform.position, Waypoints.Value[waypointIndex]) < 0.2f)
        {
            waypointIndex++;
            if(waypointIndex >= Waypoints.Value.Count)
            {
                waypointIndex = 0;
            }
        }

        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

