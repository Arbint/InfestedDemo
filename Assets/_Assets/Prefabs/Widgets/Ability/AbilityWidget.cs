using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWidget : MonoBehaviour
{
    [SerializeField] Image mIconImage;
    [SerializeField] RectTransform mScalePivot;
    [SerializeField] Image mCooldownImage;

    public GameplayAbility Ability { get; private set; }

    [SerializeField] float mScaleMaxPositionOffset;
    [SerializeField] float mMaxScale;

    Vector3 mGoalScale = Vector3.one;
    Vector3 mGoalPosition;

    bool mIsCooldownUpdating = false;
    internal void Init(GameplayAbility ability)
    {
        mIconImage.sprite = ability.AbilityIcon;
        mCooldownImage.gameObject.SetActive(false);
        Ability = ability;
        ability.onCooldownStarted += StartCooldown;
        Ability.OwnerAbilitySystemComponent.GetComponent<AttributeSet>().onAttributeChanged += AttributeChanged;
    }

    private void AttributeChanged(string s, float newValue, float oldValue, GameplayEffectSpec srcSpec)
    {
        SetCanCast(Ability.CanCast());
    }

    void SetCanCast(bool canCast)
    {
        mIconImage.color = canCast ? Color.white : new Color(0.5f,0.5f,1, 0.5f);
        mCooldownImage.color = mIconImage.color;
    }

    private void StartCooldown(float cooldownDuration)
    {
        if(mIsCooldownUpdating)
        {
            return;
        }
        
        StartCoroutine(UpdateCooldown(cooldownDuration));
    }

    IEnumerator UpdateCooldown(float cooldownDuration)
    {
        SetCanCast(false);
        mCooldownImage.gameObject.SetActive(true);
        mIsCooldownUpdating = true;
        float cooldownCounter = cooldownDuration;
        while(cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
            mCooldownImage.fillAmount = cooldownCounter/cooldownDuration;
            yield return new WaitForEndOfFrame();
        }

        mCooldownImage.gameObject.SetActive(false);
        mIsCooldownUpdating = false;
        SetCanCast(true);
    }

    internal void CastAbility()
    {
        Ability.TryActivateAbility();
    }

    internal void SetScaleUpAmt(float scaleAmt)
    {
        mGoalScale = Vector3.Lerp(Vector3.one, Vector3.one * mMaxScale, scaleAmt);
        mGoalPosition = Vector3.left * mScaleMaxPositionOffset * scaleAmt;
    }

    private void Update()
    {
        mScalePivot.localScale = mGoalScale;
        mScalePivot.localPosition = mGoalPosition;
    }
}
