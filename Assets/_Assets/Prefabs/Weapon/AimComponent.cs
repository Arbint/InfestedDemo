using UnityEngine;

public class AimComponent : MonoBehaviour
{
    [SerializeField] Transform mAimBasedTransform;
    [SerializeField] float mAimDistance = 1000f;
    [SerializeField] LayerMask mAimLayerMask;

    Vector3 GetAimDir()
    {
        Vector3 aimDir = mAimBasedTransform.forward;
        return new Vector3(aimDir.x, 0f, aimDir.z).normalized;
    }

    public GameObject GetAimTarget()
    {
        if(Physics.Raycast(mAimBasedTransform.position, 
            GetAimDir(), out RaycastHit hitInfo, mAimDistance, mAimLayerMask))
        {
            return hitInfo.collider.gameObject; 
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(mAimBasedTransform.position,
            mAimBasedTransform.position + GetAimDir() * mAimDistance);
    }
}
