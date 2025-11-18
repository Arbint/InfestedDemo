using System;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseComponent : MonoBehaviour
{
    [SerializeField] ShopItem mTestItem;
    [SerializeField] ShopSystem mShopSystem;
    List<IPurchaseHandler> mPurchaseHandlers = new List<IPurchaseHandler>();

    void Awake()
    {
        mPurchaseHandlers.AddRange(GetComponents<IPurchaseHandler>());
        Invoke("TestPurchase", 3);
    }

    void TestPurchase()
    {
        mShopSystem.TryPurchase(this, mTestItem);
    }

    public bool TryPurchase(ShopItem shopItem)
    {
        AttributeSet purchaserAttributeSet = GetComponent<AttributeSet>();
        if(!purchaserAttributeSet)
            return false;

        if(purchaserAttributeSet.Credits.CurrentValue < shopItem.Cost)          
            return false;

        purchaserAttributeSet.ApplyModifier(
            new AttributeModifier("Credit", -shopItem.Cost, EModOperation.Add, 0, false, 0, 0),
            new GameplayEffectSpec(new GameplayEffect(), gameObject, 0));

        Debug.Log($"Purchased: {shopItem.Item}");
        DispathPurchaseItem(shopItem);
        return true; 
    }

    private void DispathPurchaseItem(ShopItem shopItem)
    {
        foreach(IPurchaseHandler purchaseHandler in mPurchaseHandlers)
        {
            if(purchaseHandler.HandlePurchase(shopItem))
            {
                return;
            }
        }
    }
}
