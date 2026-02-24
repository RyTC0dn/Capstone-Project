using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DamageBoss", story: "[BossHealth] is Added or Subtracted by [Value]", category: "Action", id: "c825e2eb15d8696001799c42b58922dc")]
public partial class DamageBossAction : Action
{
    [SerializeReference] public BlackboardVariable<int> BossHealth;
    [SerializeReference] public BlackboardVariable<int> Value;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        BossHealth.Value += Value.Value;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

