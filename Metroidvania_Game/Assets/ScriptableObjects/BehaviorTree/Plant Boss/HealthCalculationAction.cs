using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Health Calculation", story: "(int) [Health] = Health [leftValue] [Operator] [rightValue]", category: "Action", id: "830c0b97b43bf14ea8ee3956413b0dd9")]
public partial class HealthCalculationAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Health;
    [SerializeReference] public BlackboardVariable<int> LeftValue;
    [SerializeReference] public BlackboardVariable<OperatorType> Operator;
    [SerializeReference] public BlackboardVariable<int> RightValue;

    protected override Status OnStart()
    {
        Health.Value = DoOperation(LeftValue.Value, Operator.Value, RightValue.Value);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Health == null || LeftValue == null || Operator == null || RightValue == null)
        {
            Debug.LogError("One or more blackboard variables are not assigned.");
            return Status.Failure;
        }
        return Status.Success;
    }

    public int DoOperation(int left, OperatorType op, int right)
    {
        switch (op)
        {
            case OperatorType.Add:
                return left + right;
            case OperatorType.Subtract:
                return left - right;
            case OperatorType.Multiply:
                return left * right;
            case OperatorType.Divide:
                return right != 0 ? left / right : 0; // Handle division by zero
            default:
                Debug.LogError("Invalid operator type.");
                return left;
        }
    }
}


