using UnityEngine;
using System;

public class Health : MonoBehaviour, IHealth
{
    // Здовоье может быть только у:
    // Игрока
    // Объекта врагов

    // Реализация для ShootemUp
    [SerializeField] private float maxHealth;
    public float MaximumHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
        }
    }

    [SerializeField] private float maximumArmor;
    public float MaximumArmor
    {
        get => maximumArmor;
        set
        {
            maximumArmor = value;
        }
    }
    private float currentArmor;

    public GameObject Instance => this.gameObject;

    public float Armor
    {
        get => Mathf.Clamp(currentArmor, 0f, maximumArmor);
        set
        {
            currentArmor = Math.Max(value, 0f);
        }
    }

    [SerializeField] private bool isArmored;
    public bool DoesHaveArmor => isArmored;
    private float currentHealth;
    public float CurrentHealth
    {
        get => Mathf.Clamp(currentHealth, 0f, maxHealth);
        set
        {
            currentHealth = Math.Max(value, 0f);
        }
    }
    // В данной реализации не используется
    public IConditionToHit conditionToHit { get; set; }

    public bool IsDied => CurrentHealth == 0f;
    public float CurrentHealthInPercentage => CurrentHealth / MaximumHealth;
    public float CurrentArmorInPercentage => Armor / MaximumArmor;

    public Action OnDamaged { get; set; }
    public Action OnArmoryDestoyed { get; set; }
    public Action OnDeath { get; set;  }
    public Action OnHealthChanged { get; set; }
    public Action OnArmorChanged { get; set; }

    public void Start()
    {
        currentHealth = MaximumHealth;
        currentArmor = MaximumArmor;
        Debug.Log($"Helath inited! hp{currentHealth} arm{currentArmor}");
    }

    public void TakeDamage(Damage damage)
    {
        Debug.Log($"Registering hit armordaamge {damage.damageArmor} healthdamage {damage.damageHealth} currentArmor {currentArmor} current health {currentHealth} is armored {isArmored}");
        if (isArmored)
        {
            Debug.Log($"Armor just hit {damage.damageArmor}");
            // Дополнительный урок
            float excessDamage = damage.damageMultiplier * damage.damageArmor - currentArmor;
            currentArmor -= damage.damageMultiplier * damage.damageArmor;
            OnArmorChanged?.Invoke();

            if (currentArmor <= 0f)
            {
                OnArmoryDestoyed?.Invoke();
                isArmored = false;

                if (excessDamage > 0f)
                {
                    currentHealth -= damage.damageMultiplier * excessDamage;
                }
            }
        }
        else
        {
            currentHealth -= damage.damageMultiplier * damage.damageHealth;
            OnHealthChanged?.Invoke();
        }
        Debug.Log($"Damage dealed hpd {damage.damageHealth} armd {damage.damageArmor} dmp {damage.damageMultiplier}. I`m hp = {currentHealth} & arm = {currentArmor}");
        OnDamaged?.Invoke();

        if (IsDied) OnDeath?.Invoke();
    }
}
