using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] float mHealth;
    [SerializeField] float mMaxHealth;

    public float Health => mHealth;
    public float MaxHealth => mMaxHealth;

    public delegate void OnHealthChange(float health, float delta, float maxHealth);
    public delegate void OnTakenDamage(float amt, GameObject instigator);
    public delegate void OnHealthEmpty();

    public event OnHealthChange onHealthChanged;
    public event OnTakenDamage onTakenDamage;
    public event OnHealthEmpty onHealthEmpty;

    public void ChangeHealth(float amt, GameObject instigator)
    {
        if (amt == 0 || mHealth == 0)
            return;

        mHealth += amt;
        mHealth = Mathf.Clamp(mHealth, 0, mMaxHealth);

        if(amt < 0)
        {
            onTakenDamage?.Invoke(amt, instigator);
        }

        onHealthChanged?.Invoke(mHealth, amt, mMaxHealth);

        if(mHealth <= 0)
        {
            onHealthEmpty?.Invoke();
        }

        Debug.Log($"Health changed by {amt} and is now {mHealth}/{mMaxHealth}");
    }
}
