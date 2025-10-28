using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileComponent : MonoBehaviour, ITeamInterface
{
    [SerializeField] float mFlightHeight = 2f;
    [SerializeField] private GameplayEffect mDamageEffect;
    [SerializeField] GameObject mExplodeCosmetic;
    [SerializeField] float mExplodeCosmeticScale = 0.1f;
    private Rigidbody mRigidBody;
    private GameObject mOwner;

    uint mTeamId;

    void Awake()
    {
        mRigidBody = GetComponent<Rigidbody>();
    }

    public void SetOwner(GameObject owner)
    {
        mOwner = owner;
        ITeamInterface ownerTeamInterface = owner.GetComponent<ITeamInterface>();
        if(ownerTeamInterface is null)
        {
            throw new Exception($"Projectile: {gameObject.name} needs a valid owner team interface!");
        }

        mTeamId = ownerTeamInterface.GetTeamId();
    }

    internal void SetDestination(Vector3 destination)
    {
        float flightTime = MathF.Sqrt(2 * mFlightHeight / Physics.gravity.magnitude);
        float verticalSpeed = mFlightHeight / flightTime;

        Vector3 destinationVector = destination - transform.position;
        destinationVector.y = 0f;
        float horizontalTravelDistance = destinationVector.magnitude;
        float horizontalTravelSpeed = horizontalTravelDistance / flightTime;

        Vector3 travelVelocity = Vector3.up * verticalSpeed + destinationVector.normalized * horizontalTravelSpeed;
        mRigidBody.AddForce(travelVelocity, ForceMode.VelocityChange);
    }

    public uint GetTeamId()
    {
        return mTeamId;
    }

    void OnTriggerEnter(Collider other)
    {
        TeamAttitute otherTeamAttitute = (this as ITeamInterface).GetTeamAttituteTowards(other.gameObject);
        if (otherTeamAttitute == TeamAttitute.Friendly)
        {
            return;
        }

        if (otherTeamAttitute == TeamAttitute.Hostile)
        {
            //do the damage
            AbilitySystemComponent otherAbilitySystemComponent = other.GetComponent<AbilitySystemComponent>();
            if(otherAbilitySystemComponent)
            {
                otherAbilitySystemComponent.ApplyGameplayEffectToSelf(new GameplayEffectSpec(mDamageEffect, mOwner, 0));
            }
        }

        Explode();
    }

    private void Explode()
    {
        GameObject explosionCosmetics = Instantiate(mExplodeCosmetic, transform.position, transform.rotation);
        explosionCosmetics.transform.localScale = Vector3.one * mExplodeCosmeticScale;
        AutoDestroy AutoDestoryComp = explosionCosmetics.AddComponent<AutoDestroy>();
        AutoDestoryComp.SetDestroyTime(1f);

        Destroy(gameObject);
    }
}
