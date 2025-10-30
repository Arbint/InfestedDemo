using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AttributeSet))]
public class AbilitySystemComponent : MonoBehaviour
{
    [SerializeField] GameplayAbility[] mInitialAbilities;
    [SerializeField] GameplayEffect[] mInitialEffects;
    List<GameplayAbility> mAbilities = new List<GameplayAbility>();


    AttributeSet mAttributeSet;

    void Awake()
    {
        mAttributeSet = GetComponent<AttributeSet>();
    }

    public void ApplyGameplayEffectToSelf(GameplayEffectSpec effectToApply)
    {
        mAttributeSet.ApplyGameplayEffect(effectToApply);
    }

    void Start()
    {
        foreach (GameplayAbility initialAbility in mInitialAbilities)
        {
            GiveAbility(initialAbility);
        }

        foreach(GameplayEffect initialEffect in mInitialEffects)
        {
            ApplyGameplayEffectToSelf(new GameplayEffectSpec(initialEffect, gameObject, 0));
        }
    }

    private void GiveAbility(GameplayAbility initialAbility)
    {
        GameplayAbility newAbility = Instantiate(initialAbility);
        newAbility.Init(this);
        mAbilities.Add(newAbility);
    }

    internal bool TryApplyCostEffect(GameplayEffect mCostGameplayEffect)
    {
        if (!CanApplyCostEffect(mCostGameplayEffect))
            return false;

        ApplyGameplayEffectToSelf(new GameplayEffectSpec(mCostGameplayEffect, gameObject, 0));
        return true;
    }

    internal bool CanApplyCostEffect(GameplayEffect mCostGameplayEffect)
    {
        return mAttributeSet.CanApplyCostEffect(mCostGameplayEffect);
    }
}
