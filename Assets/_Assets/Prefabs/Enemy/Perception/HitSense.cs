using System;
using UnityEngine;

public class HitSense : Sense
{
    void Awake()
    {
        HealthComponent ownerHealthComponent = GetComponent<HealthComponent>();
        if (ownerHealthComponent)
        {
            ownerHealthComponent.onTakenDamage += OwnerTakenDamage;
        }
    }

    private void OwnerTakenDamage(float amt, GameObject instigator)
    {
        PerceptionStimuli instigatorStimuli = instigator.GetComponent<PerceptionStimuli>();
        if (instigatorStimuli)
        {
            TryAddSensedStimuli(instigatorStimuli);
        }
    }

    protected override bool IsStimuliSensed(PerceptionStimuli stimuli)
    {
        return false;
    }
}
