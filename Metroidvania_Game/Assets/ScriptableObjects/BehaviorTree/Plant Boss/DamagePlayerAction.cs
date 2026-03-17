using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Damage Player", story: "Player [HP] depleted by set [Damage]", category: "Action", id: "2efae19069c89102ea309a4fb1d6ec3e")]
public partial class DamagePlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<PlayerHealth> HP;
    [SerializeReference] public BlackboardVariable<int> Damage;

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

