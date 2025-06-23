using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    [SerializeField] private int maxHealth;
    private int currentHealth;
    public event Action<int> OnHealthChanged;
    public event Action OnPlayerDied;
    public event Action OnEnemyDied;
    public static event Action OnAnyEnemyDied;
 

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0)
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
            OnAnyEnemyDied?.Invoke();
        }
    }
}
