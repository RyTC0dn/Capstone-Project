using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Vine_Attack", story: "Set [VineObject] at [Position] number [Int] ", category: "Action", id: "79c2707f98000ea75cc48039d62a593a")]
public partial class VineAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> VineObject;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Position;
    [SerializeReference] public BlackboardVariable<VineLogic> VineSetup;
    [SerializeReference] public BlackboardVariable<int> Int;

    private int currentPositionCount;

    protected override Status OnStart()
    {
        //If either the vine object or random position variables empty (no assigned object)
        //return status failure
        if (VineObject.Value == null || Position.Value == null) return Status.Failure;

        //Randomly choose one of the positions from the list array
        currentPositionCount = Int.Value;

        //On start, set the vine object to one of the randomly chosen positions
        VineObject.Value.transform.position = Position.Value[currentPositionCount].transform.position;

        //if (currentPositionCount == 0 || currentPositionCount == 1)
        //{
        //    VineSetup.Value.VineActions(true, false, false);
        //}
        //else if (currentPositionCount == 2)
        //{
        //    VineSetup.Value.VineActions(false, false, true);
        //}

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}