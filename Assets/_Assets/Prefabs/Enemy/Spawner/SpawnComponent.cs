using Unity.Behavior;
using UnityEditor.ShaderGraph.Configuration;
using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToSpawn;
    [SerializeField] Transform mSpawnTransform;

    PerceptionComponent mPerceptionComponent;

    void Awake()
    {
        mPerceptionComponent = GetComponent<PerceptionComponent>();
    }

    public void Spawn()
    {
        if (objectsToSpawn.Length == 0)
            return;
        int pick = Random.Range(0, objectsToSpawn.Length);

        GameObject newSpawn = Instantiate(objectsToSpawn[pick], mSpawnTransform.position, mSpawnTransform.rotation);

        GameObject target = null;
        if (mPerceptionComponent)
        {
            target = mPerceptionComponent.GetTarget();
        }

        newSpawn.GetComponent<BehaviorGraphAgent>().BlackboardReference.SetVariableValue("Target", target);
    }
}
