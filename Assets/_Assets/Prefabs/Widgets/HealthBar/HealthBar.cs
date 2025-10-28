using System;
using UnityEngine;

[RequireComponent(typeof(ValueGauge))]
public class HealthBar : UserWidget
{
    ValueGauge mValueGauge;
    [SerializeField] bool mShouldDestoryWhenHealthEmpty = true;
    private void Awake()
    {
       mValueGauge = GetComponent<ValueGauge>(); 
    }
    public override void SetOwner(GameObject owner)
    {
        base.SetOwner(owner);
        HealthComponent ownerHealthComp = owner.GetComponent<HealthComponent>();
        if (ownerHealthComp)
        {
            ownerHealthComp.onHealthChanged += OnwerHealthChanged;
            ownerHealthComp.onHealthEmpty += () =>
            {
                if (mShouldDestoryWhenHealthEmpty)
                    Destroy(gameObject);
            };

            AttributeSet ownerAttributeSet = owner.GetComponent<AttributeSet>();
            OnwerHealthChanged(ownerAttributeSet.Health.CurrentValue, 0, ownerAttributeSet.MaxHealth.CurrentValue);
        }
    }

    private void OnwerHealthChanged(float health, float delta, float maxHealth)
    {
        mValueGauge.SetPercent(health/maxHealth);
        mValueGauge.SetValueText($"{health}/{maxHealth}");
    }
}
