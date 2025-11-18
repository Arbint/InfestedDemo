using UnityEngine;

public class RotationMovementComponent : MonoBehaviour
{
    [SerializeField] float mTurnRate=20f;

    void Update()
    {
        transform.rotation = transform.rotation * Quaternion.AngleAxis(mTurnRate * Time.deltaTime, Vector3.up);
    }
}
