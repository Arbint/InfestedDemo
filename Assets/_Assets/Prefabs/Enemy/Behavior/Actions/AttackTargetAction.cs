using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackTarget", story: "[Agent] Attacks [Target]", category: "Action", id: "157a60be468a046a457f44fe70979884")]
public partial class AttackTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        IBehaviorInterface agentBehaviorInterface = Agent.Value.GetComponent<IBehaviorInterface>();
        if (agentBehaviorInterface is not null)
        {
            agentBehaviorInterface.AttackTarget(Target.Value.gameObject);
        }

        return Status.Success;
    }
}

