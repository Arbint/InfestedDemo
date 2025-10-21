using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttributeSet))]
public class AbilitySystemComponent : MonoBehaviour
{
    [SerializeField] GameplayAbility[] mInitialAbilities;
    [SerializeField] GameplayEffect[] mInitialEffects;
    List<GameplayAbility> mAbilities = new List<GameplayAbility>();

    public delegate void OnAttributeChanged(string name, float newValue, float oldValue);
    public event OnAttributeChanged onAttributeChanged;

    AttributeSet mAttributeSet;

    void Awake()
    {
        mAttributeSet = GetComponent<AttributeSet>();
    }

    public void ApplyGameplayEffectToSelf(GameplayEffect effectToApply)
    {
        foreach(AttributeModifier modifier in effectToApply.Modifiers)
        {
            ApplyModifier(modifier);
        }
    }

    private void ApplyModifier(AttributeModifier modifier)
    {
        mAttributeSet.ApplyModifier(modifier);
    }

    void Start()
    {
        foreach (GameplayAbility initialAbility in mInitialAbilities)
        {
            GiveAbility(initialAbility);
        }

        foreach(GameplayEffect initialEffect in mInitialEffects)
        {
            ApplyGameplayEffectToSelf(initialEffect);
        }
    }

    private void GiveAbility(GameplayAbility initialAbility)
    {
        GameplayAbility newAbility = Instantiate(initialAbility);
        newAbility.Init(this);
        mAbilities.Add(newAbility);
    }
}
