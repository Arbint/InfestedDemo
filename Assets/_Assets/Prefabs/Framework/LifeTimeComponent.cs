using System.Collections;
using UnityEngine;

public class LifeTimeComponent : MonoBehaviour
{
    public void SetLifeTime(float lifeTime)
    {
        StartCoroutine(StartDeath(lifeTime));
    }

    IEnumerator StartDeath(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
