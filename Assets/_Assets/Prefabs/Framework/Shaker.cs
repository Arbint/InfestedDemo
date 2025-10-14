using UnityEngine;

public interface IShakingInterface
{
    public void StartShake();
}

public class Shaker : MonoBehaviour
{
    [SerializeField] float mShakeDuration = .5f;
    [SerializeField] float mShakingMagnitude = .3f;
    [SerializeField] Transform mTransformToShake;
    [SerializeField] float mTransformLerpBackRate = 10f;
    bool mIsShaking;
    Vector3 mTransformDefaultLocalPosition;
    public void StartShake()
    {
        Debug.Log($"Shaking!");
        if (!mIsShaking)
        {
            mIsShaking = true;
            Invoke("StopShaking", mShakeDuration);
        }
    }

    void Awake()
    {
        mTransformDefaultLocalPosition = mTransformToShake.localPosition; 
    }

    void StopShaking()
    {
        mIsShaking = false;
    }

    void Update()
    {
        if (mIsShaking)
        {
            float shakeOffsetX = Random.Range(-1f, 1f) * mShakingMagnitude;
            float shakeOffsetY = Random.Range(-1f, 1f) * mShakingMagnitude;
            float shakeOffsetZ = Random.Range(-1f, 1f) * mShakingMagnitude;
            mTransformToShake.position += new Vector3(shakeOffsetX, shakeOffsetY, shakeOffsetZ); 
        }
        else
        {
            mTransformToShake.localPosition = Vector3.Lerp(mTransformToShake.localPosition,
            mTransformDefaultLocalPosition, Time.deltaTime * mTransformLerpBackRate);
        }
    }
}
