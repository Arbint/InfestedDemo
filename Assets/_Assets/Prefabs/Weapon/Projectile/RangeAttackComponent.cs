using Unity.Behavior;
using UnityEngine;

public class RangeAttackComponent : MonoBehaviour
{
    [SerializeField] ProjectileComponent mProjectilePrefab;
    [SerializeField] Transform mProjectileSpawnTransform;

    BehaviorGraphAgent mBehaviorGraphAgent;

    void Awake()
    {
        mBehaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
    }
    public void AttackPoint()
    {
        if(mBehaviorGraphAgent.BlackboardReference.GetVariable("Target", out BlackboardVariable<GameObject> targetBlackboardVariable))
        {
            GameObject target = targetBlackboardVariable.Value;
            ProjectileComponent newProjectile = Instantiate(mProjectilePrefab, mProjectileSpawnTransform.position, mProjectileSpawnTransform.rotation);
            newProjectile.transform.localScale = transform.lossyScale;

            newProjectile.SetDestination(target.transform.position);
            newProjectile.SetOwner(gameObject);
        }
    }
}
