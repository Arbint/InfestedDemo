using System.Collections.Generic;
using UnityEngine;

public class MeleeDamageComponent : MonoBehaviour
{
    [SerializeField] Transform mDamageOrigin;
    [SerializeField] float mDamageRadius = 1;
    [SerializeField] private GameplayEffect mDamageEffect;
    [SerializeField] bool mDrawDebug = true;

    ITeamInterface mTeamInterface;

    void Awake()
    {
        mTeamInterface = GetComponent<ITeamInterface>();
        if (mTeamInterface is null)
        {
            throw new System.Exception($"{gameObject.name}: need a team interface to use a Damage Component");
        }
    }

    public void AttackPoint()
    {
        Collider[] collidersInRange = Physics.OverlapSphere(mDamageOrigin.position, mDamageRadius);

        HashSet<GameObject> detectedTargets = new HashSet<GameObject>();
        foreach (Collider colliderInRange in collidersInRange)
        {
            if (detectedTargets.Contains(colliderInRange.gameObject))
                continue;

            detectedTargets.Add(colliderInRange.gameObject);
            if (mTeamInterface.GetTeamAttituteTowards(colliderInRange.gameObject) == TeamAttitute.Hostile)
            {
                AbilitySystemComponent abilitySystemComponent = colliderInRange.GetComponent<AbilitySystemComponent>();
                if (abilitySystemComponent)
                {
                    Debug.Log($"Damaging: {colliderInRange.gameObject.name}");
                    abilitySystemComponent.ApplyGameplayEffectToSelf(new GameplayEffectSpec(mDamageEffect, gameObject, 0));
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (mDrawDebug)
        {
            Gizmos.DrawWireSphere(mDamageOrigin.position, mDamageRadius);
        }
    }
}
