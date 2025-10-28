using System;
using System.Collections;
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

    void Awake()
    {
        Mana.AddValuePostProcessor(ClampMana); 
        Health.AddValuePostProcessor(ClampHealth);
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
        string attributeName = modifier.AttributeName;
        PropertyInfo propertyInfo = GetType().GetProperty(attributeName);
        if (propertyInfo == null || propertyInfo.PropertyType != typeof(GameplayAttribute))
            return;

        MethodInfo applyModiferMethodInfo = typeof(GameplayAttribute).GetMethod("AddModifier");
        float oldValue = GetPropertyCurrentValue(propertyInfo, out bool found);
        if (applyModiferMethodInfo != null)
        {
            applyModiferMethodInfo.Invoke(propertyInfo.GetValue(this), new object[] { modifier });
            float newValue = GetPropertyCurrentValue(propertyInfo, out found);
            if (oldValue != newValue)
            {
                onAttributeChanged?.Invoke(attributeName, newValue, oldValue, effectSpec); 
            }
            
            if(modifier.ModDuration != 0)
            {
                StartCoroutine(RemoveModiferAfterDuration(propertyInfo, modifier, effectSpec));
            }
        }
    }

    float GetPropertyCurrentValue(PropertyInfo propertyInfo, out bool foundAttribute)
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
}
