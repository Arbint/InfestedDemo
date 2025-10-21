using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Reference Exists", story: "[Reference] Exists [Condition]", category: "Conditions", id: "00d3be120bb639bc0908f101570a1eb7")]
public partial class ReferenceExistsCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Reference;
    [SerializeReference] public BlackboardVariable<bool> Condition;

    public override bool IsTrue()
    {
        if(Reference.Value != null && Condition.Value == true)
            return true;
        if (Reference.Value == null && Condition.Value == false)
            return true;
        return false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
