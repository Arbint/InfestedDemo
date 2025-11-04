using System;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(ValueGauge))]
public class AttributeBar : UserWidget
{
    ValueGauge mValueGauge;
    [SerializeField] string mValueName = "Health";
    [SerializeField] string mMaxValueName = "MaxHealth";
    [SerializeField] bool mShouldDestoryWhenValueEmpty = true;
    private void Awake()
    {
        mValueGauge = GetComponent<ValueGauge>();
    }

    AttributeSet mOwnerAttributeSet;

    PropertyInfo mValuePropertyInfo;
    PropertyInfo mMaxValuePropertyInfo;

    float mCachedValue;
    float mCachedMaxValue;

    public override void SetOwner(GameObject owner)
    {
        base.SetOwner(owner);
        // HealthComponent ownerHealthComp = owner.GetComponent<HealthComponent>();
        // if (ownerHealthComp)
        // {
        //     ownerHealthComp.onHealthChanged += OnwerHealthChanged;
        //     ownerHealthComp.onHealthEmpty += (instigator) =>
        //     {
        //         if (mShouldDestoryWhenHealthEmpty && instigator != owner)
        //             Destroy(gameObject);
        //     };

        //     AttributeSet ownerAttributeSet = owner.GetComponent<AttributeSet>();
        //     OnwerHealthChanged(ownerAttributeSet.Health.CurrentValue, 0, ownerAttributeSet.MaxHealth.CurrentValue);
        // }

        mOwnerAttributeSet = owner.GetComponent<AttributeSet>();

        mValuePropertyInfo = typeof(AttributeSet).GetProperty(mValueName);
        mMaxValuePropertyInfo = typeof(AttributeSet).GetProperty(mMaxValueName);

        if(mOwnerAttributeSet)
        {
            mCachedValue = mOwnerAttributeSet.GetPropertyCurrentValue(mValuePropertyInfo, out bool found);
            if(!found)
                return;
            mCachedMaxValue = mOwnerAttributeSet.GetPropertyCurrentValue(mMaxValuePropertyInfo, out found);
            if (!found)
                return;

            UpdateValue(mCachedValue, mCachedMaxValue);

            mOwnerAttributeSet.onAttributeChanged += AttributeChanged;
        }
    }

    private void AttributeChanged(string name, float newValue, float oldValue, GameplayEffectSpec srcSpec)
    {
        if(name == mValueName)
        {
            mCachedValue = newValue;
        }

        if (name == mMaxValueName)
        {
            mCachedMaxValue = newValue;
        }

        if (mCachedValue == 0 && mShouldDestoryWhenValueEmpty)
        {
            mOwnerAttributeSet.onAttributeChanged -= AttributeChanged;
            Destroy(gameObject);
        }

        UpdateValue(mCachedValue, mCachedMaxValue);
    }

    private void UpdateValue(float value, float maxValue)
    {
        mValueGauge.SetPercent(value/maxValue);
        mValueGauge.SetValueText($"{value}/{maxValue}");
    }
}
