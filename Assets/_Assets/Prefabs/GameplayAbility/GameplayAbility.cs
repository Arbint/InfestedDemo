using UnityEngine;

public abstract class GameplayAbility : ScriptableObject
{
    public AbilitySystemComponent AbilitySystemComponent { get; private set; }
    internal void Init(AbilitySystemComponent abilitySystemComponent)
    {
        AbilitySystemComponent = abilitySystemComponent;
    }
}
