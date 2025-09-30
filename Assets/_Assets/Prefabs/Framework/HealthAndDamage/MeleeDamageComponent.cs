using System.Collections.Generic;
using UnityEngine;

public class MeleeDamageComponent : MonoBehaviour
{
    [SerializeField] Transform mDamageOrigin;
    [SerializeField] float mDamageRadius = 1;
    [SerializeField] float mDamageAmount = 20f;
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
                HealthComponent targetHealthComponent = colliderInRange.GetComponent<HealthComponent>();
                if (targetHealthComponent)
                {
                    Debug.Log($"Damaging: {colliderInRange.gameObject.name}");
                    targetHealthComponent.ChangeHealth(-mDamageAmount, gameObject);
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
