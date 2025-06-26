using UnityEngine;

public class Player : SingletonMonoBehaviour<Player>
{
    [SerializeField] private Health health;
    [SerializeField] private Shield shield;

    private void OnEnable()
    {
        health.OnPlayerDied += HandlePlayerDied;
    }

    private void OnDisable()
    {
        health.OnPlayerDied -= HandlePlayerDied;
    }
    private void HandlePlayerDied()
    {
        Debug.Log("Player Died");
    }

    public void TakeDamage(int damage)
    {
        if (shield.GetCurrentShield() > 0)
        {
            shield.TakeDamage(damage);
        }
        else
        {
            health.TakeDamage(damage);
        }
    }
    
}