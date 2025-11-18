using UnityEngine;

[CreateAssetMenu(menuName = "Shop/ShopItem")]
public class ShopItem : ScriptableObject
{
    [field:SerializeField] public float Cost {get; private set;} = 20f;
    [field:SerializeField] public object Item {get; private set;}
    [field:SerializeField] public Sprite ItemIcon {get; private set;}
}
