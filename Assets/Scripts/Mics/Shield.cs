using UnityEngine;
using System;

public class Shield : MonoBehaviour
{
    public static event Action<int> OnShieldChanged;
    public static event Action<int> OnMaxShieldChanged;
    [SerializeField] private int maxShield = 100;
    [SerializeField] private float regenRate = 10f;
    [SerializeField] private float shieldRegenCooldown = 2f;
    private int currentShield;
    private float shieldRegenTimer;
    private float shieldRegenBuffer;

    private void Start()
    {
        SetShield(maxShield);
    }

    private void Update()
    {
        RegenerateShield();
    }

    private void RegenerateShield()
    {
        if (currentShield >= maxShield)
        {
            shieldRegenBuffer = 0f;
            return;
        }
        if (shieldRegenTimer > 0f)
        {
            shieldRegenTimer -= Time.deltaTime;
            return;
        }
        shieldRegenBuffer += Time.deltaTime * regenRate;
        if (shieldRegenBuffer >= 1f)
        {
            int regenAmount = Mathf.FloorToInt(shieldRegenBuffer);
            shieldRegenBuffer -= regenAmount;
            SetShield(currentShield + regenAmount);
        }
    }

    private void SetShield(int shield)
    {
        if (shield < 0)
        {
            shield = 0;
        }
        if (shield > maxShield)
        {
            shield = maxShield;
        }
        currentShield = shield;
        OnShieldChanged?.Invoke(currentShield);
    }

    public void TakeDamage(int damage)
    {
        if (currentShield <= 0)
        {
            return;
        }
        SetShield(currentShield - damage);
        shieldRegenTimer = shieldRegenCooldown;
    }

    public int GetCurrentShield()
    {
        return currentShield;
    }

    public void IncreaseMaxShield(int amount)
    {
        maxShield += amount;
        OnMaxShieldChanged?.Invoke(maxShield);
        SetShield(currentShield + amount);
    }

}
