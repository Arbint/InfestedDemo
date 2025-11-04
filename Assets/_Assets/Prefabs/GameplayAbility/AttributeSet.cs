using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class AttributeSet : MonoBehaviour
{
    [field: SerializeField] public GameplayAttribute Mana { get; private set; } = new GameplayAttribute(200);
    [field: SerializeField] public GameplayAttribute MaxMana { get; private set; } = new GameplayAttribute(200);
    [field: SerializeField] public GameplayAttribute Health { get; private set; } = new GameplayAttribute(200);
    [field: SerializeField] public GameplayAttribute MaxHealth { get; private set; } = new GameplayAttribute(200);
    [field: SerializeField] public GameplayAttribute MoveSpeed { get; private set; } = new GameplayAttribute(5);
    
    public delegate void OnAttributeChanged(string name, float newValue, float oldValue, GameplayEffectSpec srcSpec);
    public event OnAttributeChanged onAttributeChanged;

    MethodInfo mAddModifierMethodInfo;

    Dictionary<AttributeModifier, List<Coroutine>> mPeriodicalModifierRecords = new Dictionary<AttributeModifier, List<Coroutine>>();

    void Awake()
    {
        Mana.AddValuePostProcessor(ClampMana); 
        Health.AddValuePostProcessor(ClampHealth);
        mAddModifierMethodInfo = typeof(GameplayAttribute).GetMethod("AddModifier");
    }

    private float ClampHealth(float inValue)
    {
        return Math.Clamp(inValue, 0, MaxHealth.CurrentValue);
    }

    private float ClampMana(float inValue)
    {
        return Math.Clamp(inValue, 0, MaxMana.CurrentValue);
    }

    public void ApplyGameplayEffect(GameplayEffectSpec effectSpec)
    {
        if (effectSpec == null || effectSpec.Effect == null)
            return;
            
        foreach(AttributeModifier modifier in effectSpec.Effect.Modifiers)
        {
            ApplyModifier(modifier, effectSpec);
        }
    }

    internal void ApplyModifier(AttributeModifier modifier,  GameplayEffectSpec effectSpec)
    {
        PropertyInfo propertyInfo = GetType().GetProperty(modifier.AttributeName);
        if (propertyInfo == null || propertyInfo.PropertyType != typeof(GameplayAttribute))
            return;

        if (modifier.IsPeriodical())
        {
            RegisterPeriodicalModifier(modifier, propertyInfo, effectSpec);
            return;
        }

        AddModiferToAttribute(modifier, effectSpec, propertyInfo);

        if (modifier.IsTemporary())
        {
            StartCoroutine(RemoveModiferAfterDuration(propertyInfo, modifier, effectSpec));
        }
    }

    private void AddModiferToAttribute(AttributeModifier modifier, GameplayEffectSpec effectSpec, PropertyInfo propertyInfo)
    {
        float oldValue = GetPropertyCurrentValue(propertyInfo, out bool found);

        mAddModifierMethodInfo.Invoke(propertyInfo.GetValue(this), new object[] { modifier });
        float newValue = GetPropertyCurrentValue(propertyInfo, out found);
        if (oldValue != newValue)
        {
            onAttributeChanged?.Invoke(modifier.AttributeName, newValue, oldValue, effectSpec);
        }
    }

    private void RegisterPeriodicalModifier(AttributeModifier modifier, PropertyInfo propertyInfo, GameplayEffectSpec effectSpec)
    {
        AddModiferToAttribute(modifier, effectSpec, propertyInfo);
        Coroutine modifierCoroutine = StartCoroutine(ModifierPeriodicalCoroutine(modifier, propertyInfo, effectSpec));

        if (mPeriodicalModifierRecords.ContainsKey(modifier))
        {
            mPeriodicalModifierRecords[modifier].Add(modifierCoroutine);
        }
        else
        {
            mPeriodicalModifierRecords.Add(modifier, new List<Coroutine> { modifierCoroutine });
        }

        if (modifier.IsDurational())
        {
            StartCoroutine(StopDurationalCoroutine(modifier, modifierCoroutine));
        }
    }

    private IEnumerator StopDurationalCoroutine(AttributeModifier modifier, Coroutine modifierCoroutine)
    {
        yield return new WaitForSeconds(modifier.ModDuration);
        StopCoroutine(modifierCoroutine);
        if(mPeriodicalModifierRecords.ContainsKey(modifier))
        {
            mPeriodicalModifierRecords[modifier].Remove(modifierCoroutine);
        }
    }

    private IEnumerator ModifierPeriodicalCoroutine(AttributeModifier modifier, PropertyInfo propertyInfo, GameplayEffectSpec effectSpec)
    {
        while(true)
        {
            yield return new WaitForSeconds(modifier.Period);
            AddModiferToAttribute(modifier, effectSpec, propertyInfo);
        }
    }

    public float GetPropertyCurrentValue(PropertyInfo propertyInfo, out bool foundAttribute)
    {
        GameplayAttribute attribute = (GameplayAttribute)propertyInfo.GetValue(this);
        if (attribute != null)
        {
            foundAttribute = true; 
            return attribute.CurrentValue; 
        }
        
        foundAttribute = false;
        return 0;
    }
    
    IEnumerator RemoveModiferAfterDuration(PropertyInfo propertyInfo, AttributeModifier modifier, GameplayEffectSpec effectSpec)
    {
        yield return new WaitForSeconds(modifier.ModDuration);
        float oldValue = GetPropertyCurrentValue(propertyInfo, out bool found); 
        MethodInfo removeModiferMethodInfo = typeof(GameplayAttribute).GetMethod("RemoveModifier");
        if (removeModiferMethodInfo != null)
        {
            removeModiferMethodInfo.Invoke(propertyInfo.GetValue(this), new object[] { modifier });
            float newValue = GetPropertyCurrentValue(propertyInfo, out found);
            if (oldValue != newValue)
            {
                onAttributeChanged?.Invoke(modifier.AttributeName, newValue, oldValue, effectSpec);
            }
        }
    }

    internal float PreAttributeChange(PropertyInfo propertyInfo, float newValue)
    {
        if (propertyInfo.Name == nameof(Mana))
        {
            return Mathf.Clamp(newValue, 0, MaxMana.CurrentValue);
        }

        return newValue;
    }

    public bool CanApplyCostEffect(GameplayEffect mCostGameplayEffect)
    {
        Dictionary<string, float> aggregatedResult = new Dictionary<string, float>();
        foreach (AttributeModifier attributeModifier in mCostGameplayEffect.Modifiers)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(attributeModifier.AttributeName);
            if (!aggregatedResult.TryGetValue(attributeModifier.AttributeName, out float attributeValue))
            {
                attributeValue = GetPropertyCurrentValue(propertyInfo, out bool found);
                if (!found)
                {
                    return false;
                }
            }

            if (attributeModifier.ModOperation == EModOperation.Add)
            {
                attributeValue += attributeModifier.ModMagnitude;
            }

            if (attributeModifier.ModOperation == EModOperation.Mult)
            {
                attributeValue *= attributeModifier.ModMagnitude;
            }

            if (attributeModifier.ModOperation == EModOperation.Set)
            {
                attributeValue = attributeModifier.ModMagnitude;
            }

            if (attributeValue < 0)
            {
                return false;
            }

            if (aggregatedResult.ContainsKey(attributeModifier.AttributeName))
            {
                aggregatedResult[attributeModifier.AttributeName] = attributeValue;
            }
            else
            {
                aggregatedResult.Add(attributeModifier.AttributeName, attributeValue);
            }
        }

        return true;
    }
}
