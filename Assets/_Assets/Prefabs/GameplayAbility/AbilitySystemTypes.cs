using System;
using System.Collections.Generic;
using System.Linq;
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
    public AttributeModifier() : this("", 0, EModOperation.Add, 0, false, 0, 0)
    {

    }

    public AttributeModifier(
        string attributeName,
        float modMagnitude,
        EModOperation modOperation,
        float modDuration, 
        bool stackable,
        int maxStackAmt,
        float period 
    ) 
    {
        AttributeName = attributeName;
        ModMagnitude = modMagnitude;
        ModOperation = modOperation;
        ModDuration = modDuration;
        Stackable = stackable;
        MaxStackAmt = maxStackAmt;
        Period = period;
    }

    [field: SerializeField] public string AttributeName { get; private set; }
    [field: SerializeField] public float ModMagnitude { get; private set; }
    [field: SerializeField] public EModOperation ModOperation { get; private set; }
    [field: SerializeField] public float ModDuration { get; private set; }
    [field: SerializeField] public bool Stackable { get; private set; }
    [field: SerializeField] public int MaxStackAmt { get; private set; } = 0;
    [field: SerializeField] public float Period { get; private set; } = 0;

    public bool IsDurational()
    {
        return ModDuration != 0;
    }

    public bool IsPeriodical()
    {
        return Period != 0;
    }

    public bool IsTemporary()
    {
        return IsDurational() && !IsPeriodical();    
    }
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
        if(newModifier.IsTemporary())
        {
            AddTemporaryModifer(newModifier);
            return;
        }

        BaseValue = ApplyModifierToValue(newModifier, BaseValue);
        if (mValuePostProcessor != null)
        {
            BaseValue = mValuePostProcessor(BaseValue);
        }

        onModiferApplied?.Invoke(this);
    }

    private void AddTemporaryModifer(AttributeModifier newModifer)
    {
        int stackCount = GetModiferStackCount(newModifer);
        if (stackCount == 0)
        {
            mModifers.Add(newModifer);
            return;
        }

        if(newModifer.Stackable && stackCount < newModifer.MaxStackAmt)
        {
            mModifers.Add(newModifer);
        }
    }

    int GetModiferStackCount(AttributeModifier modifier)
    {
        int count = 0;
        foreach (AttributeModifier existingModifier in mModifers)
        {
            if (existingModifier == modifier)
            {
                count += 1;
            }
        }
        return count;
    }

    public void RemoveModifier(AttributeModifier modifer)
    {
        mModifers.Remove(modifer);
    }
}