using System;
using System.Collections;
using System.Reflection;
using UnityEngine;


public class AttributeSet : MonoBehaviour
{
    [field: SerializeField] public GameplayAttribute Mana { get; private set; } = new GameplayAttribute(200);
    [field: SerializeField] public GameplayAttribute MaxMana { get; private set; } = new GameplayAttribute(200);
    [field: SerializeField] public GameplayAttribute MoveSpeed { get; private set; } = new GameplayAttribute(5);

    void Awake()
    {
        Mana.AddValuePostProcessor(ClampMana); 
    }

    private float ClampMana(float inValue)
    {
        return Math.Clamp(Mana.BaseValue, 0, MaxMana.BaseValue);
    }

    internal void ApplyModifier(AttributeModifier modifier)
    {
        string attributeName = modifier.AttributeName;
        PropertyInfo propertyInfo = GetType().GetProperty(attributeName);
        if (propertyInfo == null || propertyInfo.PropertyType != typeof(GameplayAttribute))
            return;

        MethodInfo applyModiferMethodInfo = propertyInfo.GetType().GetMethod("AddModifier");
        if (applyModiferMethodInfo != null)
        {
            applyModiferMethodInfo.Invoke(propertyInfo.GetValue(this), new object[] { modifier });
            if(modifier.ModDuration != 0)
            {
                StartCoroutine(RemoveModiferAfterDuration(propertyInfo, modifier));
            }
        }
    }

    IEnumerator RemoveModiferAfterDuration(PropertyInfo propertyInfo, AttributeModifier modifier)
    {
        yield return new WaitForSeconds(modifier.ModDuration);
        MethodInfo removeModiferMethodInfo = propertyInfo.GetType().GetMethod("RemoveModifier");
        if (removeModiferMethodInfo != null)
        {
            removeModiferMethodInfo.Invoke(propertyInfo.GetValue(this), new object[] { modifier });
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
