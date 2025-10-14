using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float mDestoryTime = 2f;

    public void SetDestroyTime(float destroyTime)
    {
        mDestoryTime = destroyTime;
    }

    void Start()
    {
        Invoke("SelfDestroy", mDestoryTime);
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}