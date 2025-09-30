using UnityEngine;

public class CameraRig : MonoBehaviour
{
    Transform mFollowTransform;
    [SerializeField] float mTurnSpeed = 20f;
    float mTurnInput = 0f;

    public void SetFollowTransform(Transform transformToFollow)
    {
        mFollowTransform = transformToFollow;
    }

    public void SetTurnInput(float inputValue)
    {
        mTurnInput = inputValue;
    }

    private void LateUpdate()
    {
        transform.position = mFollowTransform.position;
        transform.Rotate(Vector3.up, mTurnInput * Time.deltaTime * mTurnSpeed);
    }
}
