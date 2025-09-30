using UnityEngine;

public class RangeSense : Sense
{
    [SerializeField] float mSenseRange = 5f;
    protected override bool IsStimuliSensed(PerceptionStimuli stimuli)
    {
        return Vector3.Distance(transform.position, stimuli.transform.position) <= mSenseRange;
    }

    protected override void DrawDebug()
    {
        base.DrawDebug();
        Gizmos.DrawWireSphere(transform.position + Vector3.up, mSenseRange);
    }
}
