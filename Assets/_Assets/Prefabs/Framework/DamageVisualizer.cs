using System.Collections;
using UnityEngine;

public class DamageVisualizer : MonoBehaviour
{
    HealthComponent mHealthComponent;
    Material mDynamicMaterial;
    [SerializeField] Renderer mRenderer;
    [SerializeField] Color mFlashColor;
    [SerializeField] float mFlashColorLerpRate = 20f;
    [SerializeField] string mFlashColorMaterialParamName = "_EmissionAddition";

    Color mDefaultEmissionColor;

    IShakingInterface mShakingInterface;

    public void SetShakingInterface(IShakingInterface shakingInterface)
    {
        mShakingInterface = shakingInterface;
    }

    void Awake()
    {
        mHealthComponent = GetComponent<HealthComponent>();
        mHealthComponent.onTakenDamage += StartFlash;

        mDynamicMaterial = new Material(mRenderer.material);
        mRenderer.material = mDynamicMaterial;

        mDefaultEmissionColor = mDynamicMaterial.GetColor(mFlashColorMaterialParamName);
        mShakingInterface = GetComponent<IShakingInterface>();
    }

    private void StartFlash(float amt, GameObject instigator)
    {
        if (Mathf.Abs((mDynamicMaterial.GetColor(mFlashColorMaterialParamName) - mDefaultEmissionColor).grayscale) < 0.1f)
        {
            mDynamicMaterial.SetColor(mFlashColorMaterialParamName, mFlashColor);
            StartCoroutine(StartFlashCoroutine());

            if (mShakingInterface is not null)
            {
                mShakingInterface.StartShake();
            }
        }
    }

    // Update is called once per frame
    IEnumerator StartFlashCoroutine()
    {
        float colorDiff = Mathf.Abs((mDynamicMaterial.GetColor(mFlashColorMaterialParamName) - mDefaultEmissionColor).grayscale);
        while (colorDiff >= 0.01f)
        {
            Color currentColor = mDynamicMaterial.GetColor(mFlashColorMaterialParamName);
            Color newColor = Color.Lerp(currentColor, mDefaultEmissionColor, mFlashColorLerpRate * Time.deltaTime);
            mDynamicMaterial.SetColor(mFlashColorMaterialParamName, newColor);
            colorDiff = Mathf.Abs((mDynamicMaterial.GetColor(mFlashColorMaterialParamName) - mDefaultEmissionColor).grayscale);
            yield return new WaitForEndOfFrame();
        }

        mDynamicMaterial.SetColor(mFlashColorMaterialParamName, mDefaultEmissionColor);
    }
}
