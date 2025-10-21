using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/GameplayEffect")]
public class GameplayEffect : ScriptableObject
{
    [SerializeField] public AttributeModifier[] mModifier;
    public AttributeModifier[] Modifiers => mModifier;
}
