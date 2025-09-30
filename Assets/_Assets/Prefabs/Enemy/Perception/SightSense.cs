using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering.Universal;

public class SightSense : Sense
{
    [SerializeField] float mSightDistance = 5;
    [SerializeField] float mSightPeripheralHalfAngle = 30f;
    [SerializeField] float mEyeHeight = 1f;
    protected override bool IsStimuliSensed(PerceptionStimuli stimuli)
    {
        //check if is in sightRange, if not return false
        if (Vector3.Distance(transform.position, stimuli.transform.position) > mSightDistance)
        {
            // Debug.Log($"Distance too far, lost track");
            return false;
        }

        //check if is blocked, if yes, return false
        Vector3 eyePosition = transform.position + Vector3.up * mEyeHeight;
        if (Physics.Raycast(eyePosition,
                           (stimuli.transform.position + Vector3.up * mEyeHeight - eyePosition).normalized,
                           out RaycastHit hitInfo,
                           mSightDistance))
        {
            if (hitInfo.collider.gameObject != stimuli.gameObject)
            {
                // Debug.Log($"Hit another object: {hitInfo.collider.gameObject}, lost track");
                return false;
            }
        }

        //check if is in sight peripheral half angle, if not return false
        if (Vector3.Angle(transform.forward, (stimuli.transform.position - transform.position).normalized) > mSightPeripheralHalfAngle)
        {
            // Debug.Log($"Angle is too big, lost track");
            return false;
        }

        return true;
    }

    protected override void DrawDebug()
    {
        base.DrawDebug();

        Vector3 eyePosition = transform.position + Vector3.up * mEyeHeight;
        //draw the sight range with a wireSphere.
        Gizmos.DrawWireSphere(eyePosition, mSightDistance);

        //draw the sight angle with 2 lines. 
        Vector3 leftEdgeDir = Quaternion.AngleAxis(mSightPeripheralHalfAngle, Vector3.up) * transform.forward;
        Vector3 rightEdgeDir = Quaternion.AngleAxis(-mSightPeripheralHalfAngle, Vector3.up) * transform.forward;

        Gizmos.DrawLine(eyePosition, eyePosition + leftEdgeDir * mSightDistance);
        Gizmos.DrawLine(eyePosition, eyePosition + rightEdgeDir * mSightDistance);
    }
}
