using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "List is Empty", story: "[List] is Empty [Condition]", category: "Conditions", id: "ff29698d3640f3664d83699b52523be6")]
public partial class ListIsEmptyCondition : Condition
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> List;
    [SerializeReference] public BlackboardVariable<bool> Condition;

    public override bool IsTrue()
    {
        if (Condition.Value == true && List.Value.Count == 0)
        {
            return true;
        }

        if (Condition.Value == false && List.Value.Count != 0)
        {
            return true;
        }

        return false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
