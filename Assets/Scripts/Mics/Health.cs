using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    [SerializeField] private int maxHealth;
    private int currentHealth;
    public static event Action<int> OnPlayerHealthChanged;
    public event Action OnPlayerDied;
    public event Action OnEnemyDied;


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
        if (isPlayer)
        {
            OnPlayerHealthChanged?.Invoke(currentHealth);
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
