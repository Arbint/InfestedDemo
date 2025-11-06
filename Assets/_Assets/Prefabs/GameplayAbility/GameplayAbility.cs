using System.Collections;
using UnityEngine;
using System;

public abstract class GameplayAbility : ScriptableObject
{
    [SerializeField] GameplayEffect mCostGameplayEffect;
    [SerializeField] Sprite mAbilityIcon;
    [SerializeField] float mCooldownDuration;
    bool mOnCooldown = false;

    public Sprite AbilityIcon => mAbilityIcon;

    public event Action<float> onCooldownStarted;

    public AbilitySystemComponent OwnerAbilitySystemComponent { get; private set; }
    internal void Init(AbilitySystemComponent abilitySystemComponent)
    {
        OwnerAbilitySystemComponent = abilitySystemComponent;
    }

    public bool CanCast()
    {
        if (IsOnCoodown())
            return false;

        if (!OwnerAbilitySystemComponent.CanApplyCostEffect(mCostGameplayEffect))
            return false;

        return true;
    }
    public bool TryActivateAbility()
    {
        if (IsOnCoodown() || !OwnerAbilitySystemComponent.CanApplyCostEffect(mCostGameplayEffect))
            return false;

        ActivateAbility();
        return true;
    }

    public virtual void ActivateAbility()
    {
        //implement in child class  
    }

    public bool IsOnCoodown()
    {
        return mOnCooldown;
    }

    protected bool CommitAbility()
    {
        if (IsOnCoodown())
            return false;

        if (!OwnerAbilitySystemComponent.TryApplyCostEffect(mCostGameplayEffect))
        {
            return false;
        }

        OwnerAbilitySystemComponent.StartCoroutine(StartCooldownCoroutine());
        return true;
    }

    private IEnumerator StartCooldownCoroutine()
    {
        mOnCooldown = true;
        onCooldownStarted?.Invoke(mCooldownDuration);
        yield return new WaitForSeconds(mCooldownDuration);
        mOnCooldown = false;
    }

    virtual protected void EndAbility()
    {
        Debug.Log($"Ability {this} Ended");
    }
}
