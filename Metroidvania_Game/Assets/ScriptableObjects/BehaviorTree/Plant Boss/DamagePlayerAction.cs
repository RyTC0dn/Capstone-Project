using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DamagePlayer", story: "Send [Damage] to Player with [GameEvent]", category: "Action", id: "a06a2b3d2212bfaebf64750073ea2fae")]
public partial class DamagePlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Damage;
    [SerializeReference] public BlackboardVariable<PlayerHealth> GameEvent;

    protected override Status OnStart()
    {
        if (GameEvent.Value == null)
            return Status.Failure;

        GameEvent.Value.TakeDamage(Damage.Value, null);

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