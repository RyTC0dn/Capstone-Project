using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CurrentState Change", story: "[CurrentState] has changed", category: "Conditions", id: "684a331f914d73f5f52d3b2d92f5b628")]
public partial class CurrentStateChangeCondition : Condition
{
    [SerializeReference] public BlackboardVariable<CurrentState> CurrentState;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
