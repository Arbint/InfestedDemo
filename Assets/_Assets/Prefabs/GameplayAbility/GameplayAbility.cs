using System.Collections;
using UnityEngine;

public abstract class GameplayAbility : ScriptableObject
{
    [SerializeField] GameplayEffect mCostGameplayEffect;
    [SerializeField] float mCooldownDuration;
    bool mOnCooldown = false;
    public AbilitySystemComponent AbilitySystemComponent { get; private set; }
    internal void Init(AbilitySystemComponent abilitySystemComponent)
    {
        AbilitySystemComponent = abilitySystemComponent;
    }

    public bool TryActivateAbility()
    {
        if (IsOnCoodown() || !AbilitySystemComponent.CanApplyCostEffect(mCostGameplayEffect))
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

        if (!AbilitySystemComponent.TryApplyCostEffect(mCostGameplayEffect))
        {
            return false;
        }

        AbilitySystemComponent.StartCoroutine(StartCooldownCoroutine());
        return true;
    }

    private IEnumerator StartCooldownCoroutine()
    {
        mOnCooldown = true;
        yield return new WaitForSeconds(mCooldownDuration);
        mOnCooldown = false;
    }
}
