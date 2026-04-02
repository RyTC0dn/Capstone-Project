using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BossDeath", story: "On Boss Death, [SceneInfo] isBossKilled is [boolean]", category: "Action", id: "778195ab11987c1d595dd18ff06d7690")]
public partial class BossDeathAction : Action
{
    [SerializeReference] public BlackboardVariable<SceneInfo> SceneInfo;
    [SerializeReference] public BlackboardVariable<bool> Boolean;
    protected override Status OnStart()
    {
        if (SceneInfo == null) return Status.Failure;

        //On start, set the scene info variable
        SceneInfo.Value.isBossKilled = Boolean.Value;

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