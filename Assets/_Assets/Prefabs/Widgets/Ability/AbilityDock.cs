using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityDock : UserWidget, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] AbilityWidget mAbilityWidgetPrefab;

    List<AbilityWidget> mAbilityWidgets = new List<AbilityWidget>();

    PointerEventData mPointerEventData;

    [SerializeField] float mScaleRange = 100f;
    [SerializeField] float mScaleMaxAmt = 1.5f;
    [SerializeField] private float mScaleLerpRate = 20f;
    
    Vector3 mScaleGoal = Vector3.one;

    public void OnPointerDown(PointerEventData eventData)
    {
        mPointerEventData = eventData;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        AbilityWidget widgetUnderPointer = GetAbilityWidgetUnderPointer(eventData);
        if(widgetUnderPointer)
        {
            widgetUnderPointer.CastAbility();
        }
        mPointerEventData = null;
    }

    public override void SetOwner(GameObject owner)
    {
        base.SetOwner(owner);
        AbilitySystemComponent ownerAbilitySystemComponent = owner.GetComponent<AbilitySystemComponent>();
        ownerAbilitySystemComponent.onNewAbilityGiven += NewAbilityGiven;        
    }

    private void NewAbilityGiven(GameplayAbility ability)
    {
        AbilityWidget newAbilityWidget = Instantiate<AbilityWidget>(mAbilityWidgetPrefab, transform);
        newAbilityWidget.Init(ability);
        mAbilityWidgets.Add(newAbilityWidget);
    }

    AbilityWidget GetAbilityWidgetUnderPointer(PointerEventData pointerEventData)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        foreach(RaycastResult raycastResult in raycastResults)
        {
            AbilityWidget foundAbilityWidget = raycastResult.gameObject.GetComponent<AbilityWidget>();
            if(foundAbilityWidget)
            {
                return foundAbilityWidget;
            }
        }

        return null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        mPointerEventData = eventData;
        Debug.Log($"drag at position: {eventData.position}");
    }

    private void Update()
    {
        if(mPointerEventData != null)
        {
            ScaleAbilityWidgets();
            mScaleGoal = Vector3.one * mScaleMaxAmt; 
        }else
        {
            ResetAbilityWidgetScales();
            mScaleGoal = Vector3.one;
        }
        
        transform.localScale = Vector3.Lerp(transform.localScale, mScaleGoal, Time.deltaTime * mScaleLerpRate);
    }

    private void ResetAbilityWidgetScales()
    {
        foreach(AbilityWidget abilityWidget in mAbilityWidgets)
        {
            abilityWidget.SetScaleUpAmt(0);
        }
    }

    void ScaleAbilityWidgets()
    {
        float pointerYPos = mPointerEventData.position.y;
        
        foreach(AbilityWidget abilityWidget in mAbilityWidgets)
        {
            float widgetYPos = abilityWidget.transform.position.y;
            float touchDistance = Mathf.Abs(pointerYPos - widgetYPos);
            // Debug.Log($"Widget position Y: {widgetYPos}, touch Position Y: {pointerYPos}, distance is: {touchDistance}");
            float scaleAmt = 0f;
            if(touchDistance < mScaleRange)
            {
                scaleAmt = 1 - touchDistance / mScaleRange;
            }

            abilityWidget.SetScaleUpAmt(scaleAmt);
        }
    }
}
