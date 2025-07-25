using UnityEngine;
using System;
public class Player : SingletonMonoBehaviour<Player>
{
    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private Shield shield;
    [SerializeField] private MoneyWallet moneyWallet;
    [SerializeField] private ActiveWeapon activeWeapon;
    [SerializeField] private float screenShakeStrength = 1.0f;

    public event Action<float> OnPlayerDamaged;

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
        OnPlayerDamaged?.Invoke(screenShakeStrength);

    }

    public Health GetHealth()
    {
        return health;
    }

    public Shield GetShield()
    {
        return shield;
    }

    public MoneyWallet GetMoneyWallet()
    {
        return moneyWallet;
    }

    public ActiveWeapon GetActiveWeapon()
    {
        return activeWeapon;
    }
    
}