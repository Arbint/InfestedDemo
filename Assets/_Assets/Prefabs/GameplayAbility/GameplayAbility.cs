using System.Collections;
using UnityEngine;

public abstract class GameplayAbility : ScriptableObject
{
    [SerializeField] GameplayEffect mCostGameplayEffect;
    [SerializeField] float mCooldownDuration;
    bool mOnCooldown = false;
    public AbilitySystemComponent OwnerAbilitySystemComponent { get; private set; }
    internal void Init(AbilitySystemComponent abilitySystemComponent)
    {
        OwnerAbilitySystemComponent = abilitySystemComponent;
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
        yield return new WaitForSeconds(mCooldownDuration);
        mOnCooldown = false;
    }

    virtual protected void EndAbility()
    {
        Debug.Log($"Ability {this} Ended");
    }
}
