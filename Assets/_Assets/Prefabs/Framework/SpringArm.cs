using UnityEngine;

[ExecuteAlways]
public class SpringArm : MonoBehaviour
{
    [SerializeField] Transform mChildTransform;
    [SerializeField] float mArmLength = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mChildTransform.position = transform.position - transform.forward * mArmLength; 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(mChildTransform.position, transform.position); 
    }
}
