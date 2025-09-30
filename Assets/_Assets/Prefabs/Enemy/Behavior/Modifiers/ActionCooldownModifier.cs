using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ActionCooldown", story: "Cooldown After [Duration] Seconds", category: "Flow", id: "c97bcfe60d51aa5ddba856c442765e17")]
public partial class ActionCooldownModifier : Modifier
{
    [SerializeReference] public BlackboardVariable<float> Duration;

    float mLastExecutionTime = -1f;

    protected override Status OnStart()
    {
        if (mLastExecutionTime < 0 || Time.timeSinceLevelLoad - mLastExecutionTime > Duration.Value)
        {
            mLastExecutionTime = Time.timeSinceLevelLoad;
            return StartNode(Child);
        }

        return Status.Success;
    }
}

