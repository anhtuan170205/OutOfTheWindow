using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public static event Action<int> OnPlayerHealthChanged;
    public static event Action<int> OnPlayerMaxHealthChanged;
    public event Action OnPlayerDied;
    public event Action OnEnemyDied;

    [SerializeField] private bool isPlayer;
    [SerializeField] private int maxHealth;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        SetHealth(currentHealth - damage);
        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isPlayer)
        {
            OnPlayerDied?.Invoke();
        }
        else
        {
            OnEnemyDied?.Invoke();
        }
    }

    private void SetHealth(int health)
    {
        if (health < 0)
        {
            health = 0;
        }
        currentHealth = health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (isPlayer)
        {
            OnPlayerHealthChanged?.Invoke(currentHealth);
        }
    }

    public void Heal(int amount)
    {
        SetHealth(currentHealth + amount);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        OnPlayerMaxHealthChanged?.Invoke(maxHealth);
        SetHealth(currentHealth + amount);
    }
}
