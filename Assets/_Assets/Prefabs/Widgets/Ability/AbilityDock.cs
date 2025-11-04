using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDock : UserWidget
{
    [SerializeField] AbilityWidget mAbilityWidgetPrefab;

    List<AbilityWidget> mAbilityWidgets = new List<AbilityWidget>();

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
}
