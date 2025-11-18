using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilitySystem/GameplayEffect")]
public class GameplayEffect : ScriptableObject
{
    [SerializeField] public List<AttributeModifier> mModifiers = new List<AttributeModifier>();
    public List<AttributeModifier> Modifiers => mModifiers;
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