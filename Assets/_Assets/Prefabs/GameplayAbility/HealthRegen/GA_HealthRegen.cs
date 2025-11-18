using UnityEngine;

[CreateAssetMenu(menuName = "GameplayAbility/Health Regen")]
public class GA_HealthRegen : GameplayAbility
{
    [SerializeField] float mRegenTotalAmt = 50f;
    [SerializeField] float mRegenDuration = 4f;
    [SerializeField] float mUpdateInterval = 0.5f;

    GameplayEffect mRegenEffect;

    public override void Init(AbilitySystemComponent abilitySystemComponent)
    {
        base.Init(abilitySystemComponent);
        mRegenEffect = new GameplayEffect();
        float modMag = mRegenTotalAmt / (mRegenDuration/mUpdateInterval+1);
        AttributeModifier regenModifier = new AttributeModifier(
            "Health", 
            modMag,
            EModOperation.Add,
            mRegenDuration,
            false,
            0,
            mUpdateInterval);

        mRegenEffect.Modifiers.Add(regenModifier);
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        if(!CommitAbility())
        {
            EndAbility();
            return;
        }

        OwnerAbilitySystemComponent.ApplyGameplayEffectToSelf(
            new GameplayEffectSpec(mRegenEffect, OwnerAbilitySystemComponent.gameObject, 0)
        );

        EndAbility();
    }
}
