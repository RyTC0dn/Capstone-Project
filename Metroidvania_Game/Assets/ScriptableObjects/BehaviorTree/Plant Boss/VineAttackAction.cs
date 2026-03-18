using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Vine_Attack", story: "Set [VineObject] at [RandomPosition]", category: "Action", id: "79c2707f98000ea75cc48039d62a593a")]
public partial class VineAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> VineObject;
    [SerializeReference] public BlackboardVariable<List<GameObject>> RandomPosition;
    [SerializeReference] public BlackboardVariable<float> lifeTimeCounter;

    private int currentPositionCount;

    protected override Status OnStart()
    {
        //If either the vine object or random position variables empty (no assigned object)
        //return status failure
        if (VineObject.Value == null || RandomPosition.Value == null) return Status.Failure;

        //Randomly choose one of the positions from the list array
        currentPositionCount = UnityEngine.Random.Range(0, RandomPosition.Value.Count);

        //On start, set the vine object to one of the randomly chosen positions
        VineObject.Value.transform.position = RandomPosition.Value[currentPositionCount].transform.position;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (lifeTimeCounter.Value < 0)
        {
            lifeTimeCounter.Value++;
            return Status.Running;
        }

        //VineObject.Value.
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}