using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    // [SerializeField] float mHealth;
    // [SerializeField] float mMaxHealth;
    //
    // public float Health => mHealth;
    // public float MaxHealth => mMaxHealth;

    AttributeSet mAttributeSet;
    
    public delegate void OnHealthChange(float health, float delta, float maxHealth);
    public delegate void OnTakenDamage(float amt, GameObject instigator);
    public delegate void OnHealthEmpty(GameObject instigator);

    public event OnHealthChange onHealthChanged;
    public event OnTakenDamage onTakenDamage;
    public event OnHealthEmpty onHealthEmpty;

    private void Awake()
    {
        mAttributeSet = GetComponent<AttributeSet>();
        mAttributeSet.onAttributeChanged += AttributeChanged;
    }

    private void AttributeChanged(string attributeName, float newValue, float oldValue, GameplayEffectSpec effectSpec)
    {
        if (attributeName == "Health")
        {
            float delta = newValue - oldValue;
            if (delta < 0)
            {
                onTakenDamage?.Invoke(delta, effectSpec.Instigator); 
            }
            
            onHealthChanged?.Invoke(newValue, delta, mAttributeSet.MaxHealth.CurrentValue);

            if (newValue == 0)
            {
                onHealthEmpty?.Invoke(effectSpec.Instigator);
            }
        }
    }

    // public void ChangeHealth(float amt, GameObject instigator)
    // {
    //     if (amt == 0 || mHealth == 0)
    //         return;
    //
    //     mHealth += amt;
    //     mHealth = Mathf.Clamp(mHealth, 0, mMaxHealth);
    //
    //     if(amt < 0)
    //     {
    //         onTakenDamage?.Invoke(amt, instigator);
    //     }
    //
    //     onHealthChanged?.Invoke(mHealth, amt, mMaxHealth);
    //
    //     if(mHealth <= 0)
    //     {
    //         onHealthEmpty?.Invoke();
    //     }
    //
    //     Debug.Log($"Health changed by {amt} and is now {mHealth}/{mMaxHealth}");
    // }
}
