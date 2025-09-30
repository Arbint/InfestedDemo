using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] RectTransform thumbStickTrans;
    [SerializeField] RectTransform backgroundTrans;

    Vector2 mInputValue;
    public Vector2 InputValue => mInputValue;

    public delegate void OnInputValueChanged(Vector2 inputValue);
    public event OnInputValueChanged onInputValueChanged;

    public delegate void OnTapped();
    public event OnTapped onTapped;

    bool mWasDragging;
    public void OnPointerDown(PointerEventData eventData)
    {
        backgroundTrans.position= eventData.position;
        mWasDragging = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        backgroundTrans.localPosition = Vector2.zero;
        thumbStickTrans.localPosition = Vector2.zero;
        mInputValue = Vector2.zero;
        onInputValueChanged?.Invoke(mInputValue);
        if(!mWasDragging)
        {
            onTapped?.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 centerPosition = backgroundTrans.position;
        Vector2 stickOffset = eventData.position - centerPosition;
        stickOffset = Vector2.ClampMagnitude(stickOffset, backgroundTrans.sizeDelta.x/2f);

        thumbStickTrans.position = centerPosition + stickOffset;
        mInputValue = stickOffset/(backgroundTrans.sizeDelta.x/2f);
        onInputValueChanged?.Invoke(mInputValue);

        mWasDragging=true;
    }
}
