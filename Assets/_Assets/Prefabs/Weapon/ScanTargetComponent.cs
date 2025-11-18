using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ScanTargetComponent : MonoBehaviour, ITeamInterface
{
    [SerializeField] Transform mVisualAttachmentTransform;
    private uint mTeamId;
    public uint GetTeamId()
    {
        return mTeamId;
    }

    public event Action<GameObject> onTargetAquired;

    public void StartScan(float scanRadius, float scanDuration, uint teamId, GameObject visualAttachment = null)
    {
        mTeamId = teamId;

        if(visualAttachment)
        {
            visualAttachment.transform.SetParent(mVisualAttachmentTransform, false);
        }

        StartCoroutine(StartScanCoroutine(scanRadius, scanDuration));
    }

    IEnumerator StartScanCoroutine(float scanRadius, float scanDuration)
    {
        float timeCounter = 0;
        float scaleValue = 0f;
        float scaleRate = scanRadius / scanDuration;
        while(timeCounter < scanDuration)
        {
            transform.localScale = Vector3.one * scaleValue;
            timeCounter += Time.deltaTime;
            scaleValue += scaleRate * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == null)
            return;

        if((this as ITeamInterface).GetTeamAttituteTowards(other.gameObject)==TeamAttitute.Hostile)
        {
            onTargetAquired?.Invoke(other.gameObject);
        }
    }
}
