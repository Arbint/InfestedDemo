using UnityEngine;

[CreateAssetMenu(menuName = "GameplayAbility/GA_SpeedUp")]
public class GA_SpeedUp : GameplayAbility
{
    [SerializeField] GameplayEffect mSpeedUpEffect;
    public override void ActivateAbility()
    {
        base.ActivateAbility();
        if (!CommitAbility())
        {
            EndAbility();
            return;
        }

        OwnerAbilitySystemComponent.ApplyGameplayEffectToSelf(new GameplayEffectSpec(mSpeedUpEffect, OwnerAbilitySystemComponent.gameObject, 0)); 
    }
}
