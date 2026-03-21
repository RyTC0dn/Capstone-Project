using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TriggerCollider", story: "Check if [Collider] has [Tag]", category: "Action/Conditional", id: "73c87874f01db5b58bd60cde3c28b62e")]
public partial class TriggerColliderAction : Action
{
    [SerializeReference] public BlackboardVariable<Collider2D> Collider;
    [SerializeReference] public BlackboardVariable<string> Tag;

    protected override Status OnStart()
    {
        if (Collider.Value == null || Tag.Value == null) return Status.Failure;

        if (Collider.Value.CompareTag(Tag.Value)) return Status.Success;
        else return Status.Failure;

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