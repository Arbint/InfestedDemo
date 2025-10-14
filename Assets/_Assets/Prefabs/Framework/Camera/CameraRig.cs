using UnityEngine;

[RequireComponent(typeof(Shaker))]
public class CameraRig : MonoBehaviour, IShakingInterface
{
    Transform mFollowTransform;
    [SerializeField] float mTurnSpeed = 20f;
    float mTurnInput = 0f;

    Shaker mShaker;

    void Awake()
    {
        mShaker = GetComponent<Shaker>();
    }

    public void SetFollowTransform(Transform transformToFollow)
    {
        mFollowTransform = transformToFollow;
    }

    public void SetTurnInput(float inputValue)
    {
        mTurnInput = inputValue;
    }

    public void StartShake()
    {
        mShaker.StartShake();
    }

    private void LateUpdate()
    {
        transform.position = mFollowTransform.position;
        transform.Rotate(Vector3.up, mTurnInput * Time.deltaTime * mTurnSpeed);
    }
}
