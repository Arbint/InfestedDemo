using UnityEngine;

public class Socket : MonoBehaviour
{
    [SerializeField] string mSocketName;

    public bool IsFor(string socketName)
    {
        return mSocketName == socketName;
    }
}
