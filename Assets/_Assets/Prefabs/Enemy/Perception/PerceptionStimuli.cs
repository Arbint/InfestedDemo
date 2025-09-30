using Unity.VisualScripting;
using UnityEngine;

public class PerceptionStimuli : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sense.RegisterStimuli(this);     
    }

    void OnDestroy()
    {
        Sense.UnReisterStimuli(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
