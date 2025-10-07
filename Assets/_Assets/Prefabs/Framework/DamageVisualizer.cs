using UnityEngine;

public class DamageVisualizer : MonoBehaviour
{

    HealthComponent mHealthComponent;
    Material mDynamicMaterial;
    [SerializeField] Renderer mRenderer;
    [SerializeField] Color mFlashColor;
    [SerializeField] string mFlashColorMaterialParamName = "_EmissionAddition"; 

    Color mDefaultEmissionColor;

    void Awake()
    {
        mHealthComponent = GetComponent<HealthComponent>();
        mHealthComponent.onTakenDamage += StartFlash;

        mDynamicMaterial = new Material(mRenderer.material);
        mRenderer.material = mDynamicMaterial;

        mDefaultEmissionColor = mDynamicMaterial.GetColor(mFlashColorMaterialParamName);
    }

    private void StartFlash(float amt, GameObject instigator)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
