using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Vine_Attack", story: "Spawn [vines] at [target] locations", category: "Action", id: "f3564bc617bc3b1aff98329c555cff9a")]
public partial class VineAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Vines;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    protected override Status OnStart()
    {
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

