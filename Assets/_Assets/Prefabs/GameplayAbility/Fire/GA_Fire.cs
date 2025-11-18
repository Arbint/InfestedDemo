using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GameplayAbility/Fire")]
public class GA_Fire : GameplayAbility
{
    [SerializeField] ScanTargetComponent mScanTargetComponentPrefab;
    [SerializeField] float mScanRadius=10f;
    [SerializeField] float mScanDuration=.5f;
    [SerializeField] float mTotalDamageAmt = 40f;
    [SerializeField] float mDamageDuration = 3f;
    [SerializeField] float mDamageInterval = 1f;

    [SerializeField] GameObject mFireScanVisualPrefab;
    [SerializeField] GameObject mBuringVisualPrefab;

    GameplayEffect mDamageEffect;

    public override void Init(AbilitySystemComponent abilitySystemComponent)
    {
        base.Init(abilitySystemComponent);
        mDamageEffect = new GameplayEffect();
        float mDamageModAmt = mTotalDamageAmt/(mDamageDuration/mDamageInterval+1);
        mDamageEffect.Modifiers.Add(new AttributeModifier(
        "Health", 
        -mDamageModAmt, 
        EModOperation.Add,
        mDamageDuration,
        true,
        2,
        mDamageInterval
        ));
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        if(!CommitAbility())
        {
            EndAbility();
            return;
        }

        ScanTargetComponent scanTargetComponent = Instantiate<ScanTargetComponent>(mScanTargetComponentPrefab,
            OwnerAbilitySystemComponent.transform
        );

        scanTargetComponent.onTargetAquired += DamageTarget;

        scanTargetComponent.StartScan(mScanRadius, mScanDuration, 
                OwnerAbilitySystemComponent.GetComponent<ITeamInterface>().GetTeamId(),
                Instantiate(mFireScanVisualPrefab)
        );
    }

    private void DamageTarget(GameObject target)
    {
        Debug.Log($"Found Target: {target.name}");
        AbilitySystemComponent targetASC = target.GetComponent<AbilitySystemComponent>();
        if(targetASC)
        {
            targetASC.ApplyGameplayEffectToSelf(new GameplayEffectSpec(mDamageEffect, OwnerAbilitySystemComponent.gameObject, 0));
            GameObject buringVisual = Instantiate(mBuringVisualPrefab, targetASC.transform);
            LifeTimeComponent buriningLifeTimeComp = buringVisual.AddComponent<LifeTimeComponent>();
            buriningLifeTimeComp.SetLifeTime(mDamageDuration);
        }
    }
}
