using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValueGauge : MonoBehaviour
{
    [SerializeField] Slider mSlider;
    [SerializeField] TextMeshProUGUI mValueText;

    public void SetPercent(float value)
    {
        if (value < 0 || value > 1)
        {
            return;
        }

        mSlider.value = value;
    }

    public void SetValueText(string text)
    {
        if(mValueText)
            mValueText.SetText(text);
    }
}
