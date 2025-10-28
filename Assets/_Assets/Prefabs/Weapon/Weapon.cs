using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] string mAttachSocketName;
    [SerializeField] AnimatorOverrideController mAnimatorOverrideController;
    [SerializeField] AimComponent mAimComponent;
    [SerializeField] GameplayEffect  mDamageEffect;
    [SerializeField] ParticleSystem mProjectileParticleSystem;

    Animator mOwnerAnimator;
    public string AttachSocketName => mAttachSocketName;
    public GameObject Owner
    {
        get;
        private set;
    }

    public void Init(GameObject owner)
    {
        Owner = owner;
        mOwnerAnimator = owner.GetComponent<Animator>(); 
        UnEquip();
    }

    internal void Equip()
    {
        gameObject.SetActive(true);
        mOwnerAnimator.runtimeAnimatorController = mAnimatorOverrideController;
    }

    internal void Fire()
    {
        GameObject aimTarget = mAimComponent.GetAimTarget();
        if (mProjectileParticleSystem)
        {
            mProjectileParticleSystem.Emit(mProjectileParticleSystem.emission.GetBurst(0).maxCount);
        }

        DamageGameObject(aimTarget);
    }

    internal void UnEquip()
    {
        gameObject.SetActive(false);
    }

    void DamageGameObject(GameObject objToDamage)
    {
        if (!objToDamage)
            return;

        AbilitySystemComponent abilitySystemComponent = objToDamage.GetComponent<AbilitySystemComponent>();

        if (!abilitySystemComponent)
            return;

        abilitySystemComponent.ApplyGameplayEffectToSelf(new GameplayEffectSpec(mDamageEffect, Owner, 0));
    }
}
