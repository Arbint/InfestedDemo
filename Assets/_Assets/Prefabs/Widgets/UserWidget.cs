using UnityEngine;

public abstract class UserWidget : MonoBehaviour
{
    public GameObject Ownwer { get; private set; }
    public virtual void SetOwner(GameObject owner)
    {
        Ownwer = owner;
    }
}
