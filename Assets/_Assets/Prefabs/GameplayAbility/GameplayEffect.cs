using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/GameplayEffect")]
public class GameplayEffect : ScriptableObject
{
    [SerializeField] public AttributeModifier[] mModifier;
    public AttributeModifier[] Modifiers => mModifier;
}

public class GameplayEffectSpec
{
    public GameplayEffect Effect { get; private set; }
    public GameObject Instigator { get; private set; }
    public int Level { get; private set; }

    public GameplayEffectSpec(GameplayEffect effect, GameObject instigator, int level)
    {
        Effect = effect;
        Instigator = instigator;
        Level = level;
    }
}