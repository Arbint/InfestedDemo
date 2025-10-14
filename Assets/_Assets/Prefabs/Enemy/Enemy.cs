using System;
using Unity.Behavior;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(PerceptionComponent))]
public class Enemy : MonoBehaviour, IBehaviorInterface, ITeamInterface
{
    [SerializeField] uint mTeamId = 1;
    [SerializeField] float mAttackRange = 1;
    [SerializeField] float mMoveSpeed = 3;
    [SerializeField] float mAttackCooldown = 1;
    private Animator mAnimator;
    int mAttackAnimatorTriggerHash = Animator.StringToHash("Attack");

    int mDeadAnimatorTriggerHash = Animator.StringToHash("Dead");

    private HealthComponent mHealthComponent;
    private PerceptionComponent mPerceptionComponent;
    private BehaviorGraphAgent mBehaviorGraphAgent;

    GameObject mTarget;
    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
        mHealthComponent = GetComponent<HealthComponent>();
        mHealthComponent.onHealthEmpty += StartDeath;
        mPerceptionComponent = GetComponent<PerceptionComponent>();
        mPerceptionComponent.onTargetUpdated += TargetUpdated;

        mBehaviorGraphAgent = GetComponent<BehaviorGraphAgent>();

        mBehaviorGraphAgent.BlackboardReference.SetVariableValue("MoveSpeed", mMoveSpeed);
        mBehaviorGraphAgent.BlackboardReference.SetVariableValue("AttackRange", mAttackRange);
        mBehaviorGraphAgent.BlackboardReference.SetVariableValue("AttackCooldownDuration", mAttackCooldown);
    }

    private void TargetUpdated(GameObject target, bool wasSuccessfullySensed)
    {
        if (wasSuccessfullySensed)
        {
            mTarget = target;
            mBehaviorGraphAgent.BlackboardReference.SetVariableValue<GameObject>("Target", mTarget);
        }
        else
        {
            mBehaviorGraphAgent.BlackboardReference.SetVariableValue<GameObject>("Target", null);
            mBehaviorGraphAgent.BlackboardReference.SetVariableValue<Vector3>("TargetLastSeenPosition",
            mTarget.transform.position);

            mBehaviorGraphAgent.BlackboardReference.SetVariableValue<bool>("HasLastSeenPosition", true);
            mTarget = null;
        }
    }

    private void StartDeath()
    {
        mAnimator.SetTrigger(mDeadAnimatorTriggerHash);
    }

    public void DeathAnimationFinished()
    {
        Destroy(gameObject);
    }


    void OnDrawGizmos()
    {
        if (mTarget)
        {
            Gizmos.DrawLine(transform.position, mTarget.transform.position);
            Gizmos.DrawWireSphere(mTarget.transform.position, 0.5f);
        }
    }

    public void AttackTarget(GameObject target)
    {
        // Debug.Log($"Attacking: {target.name}");
        mAnimator.SetTrigger(mAttackAnimatorTriggerHash);
    }

    public uint GetTeamId()
    {
        return mTeamId;
    }
}
