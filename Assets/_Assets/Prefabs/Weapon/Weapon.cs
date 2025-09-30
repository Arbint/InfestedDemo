using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] string mAttachSocketName;
    [SerializeField] AnimatorOverrideController mAnimatorOverrideController;
    [SerializeField] AimComponent mAimComponent;
    [SerializeField] float mDamage = 5f;

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

        HealthComponent objectHealthComponent = objToDamage.GetComponent<HealthComponent>();

        if (!objectHealthComponent)
            return;

        objectHealthComponent.ChangeHealth(-mDamage, Owner);
    }
}
