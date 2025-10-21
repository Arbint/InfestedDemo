using System;
using System.Collections.Generic;
using UnityEngine;

public enum EModOperation
{
    Add,
    Mult,
    Set
}

[Serializable]
public class AttributeModifier
{
    [field: SerializeField] public string AttributeName { get; private set; }
    [field: SerializeField] public float ModMagnitude { get; private set; }
    [field: SerializeField] public EModOperation ModOperation { get; private set; }
    [field: SerializeField] public float ModDuration { get; private set; }
}

[Serializable]
public class GameplayAttribute
{
    [field: SerializeField] public float BaseValue { get; private set; }

    public Action<GameplayAttribute> onModiferApplied;

    public delegate float ValueProcessor(float inValue);
    ValueProcessor mValuePostProcessor;

    public void AddValuePostProcessor(ValueProcessor processor)
    {
        mValuePostProcessor = processor;
    }

    public GameplayAttribute(float baseValue)
    {
        BaseValue = baseValue;
    }

    public float CurrentValue
    {
        get
        {
            return CalculateCurrentValue();
        }
    }

    private float CalculateCurrentValue()
    {
        float outValue = BaseValue;
        foreach (AttributeModifier modifier in mModifers)
        {
            outValue = ApplyModifierToValue(modifier, outValue);
        }

        if (mValuePostProcessor != null)
        {
            outValue = mValuePostProcessor(outValue);
        }

        return outValue;
    }

    public static float ApplyModifierToValue(AttributeModifier modifier, float value)
    {
        if (modifier.ModOperation == EModOperation.Add)
        {
            return value + modifier.ModMagnitude;
        }

        if (modifier.ModOperation == EModOperation.Mult)
        {
            return value * modifier.ModMagnitude;
        }

        if (modifier.ModOperation == EModOperation.Set)
        {
            return modifier.ModMagnitude;
        }

        return value;
    }

    List<AttributeModifier> mModifers = new List<AttributeModifier>();

    public void AddModifier(AttributeModifier newModifier)
    {
        if (newModifier.ModDuration == 0)
        {
            BaseValue = ApplyModifierToValue(newModifier, BaseValue);
            if (mValuePostProcessor != null)
            {
                BaseValue = mValuePostProcessor(BaseValue);
            }

            onModiferApplied?.Invoke(this);
        }
        else
        {
            AddTemporaryModifer(newModifier);
        }
    }

    private void AddTemporaryModifer(AttributeModifier newModifer)
    {
        mModifers.Add(newModifer);
    }

    internal void RemoveModifier(AttributeModifier modifer)
    {
        mModifers.Remove(modifer);
    }
}