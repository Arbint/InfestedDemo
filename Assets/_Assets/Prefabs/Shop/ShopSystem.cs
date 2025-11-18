using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Shop/ShopSystem")]
public class ShopSystem : ScriptableObject
{
    [SerializeField] ShopItem[] mShopItems;

    public bool TryPurchase(PurchaseComponent puchaser, ShopItem shopItem)
    {
        return puchaser.TryPurchase(shopItem);
    }
}
