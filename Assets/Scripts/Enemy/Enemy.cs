using UnityEngine;
using UnityEngine.AI;
using System;

public abstract class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Health health;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] protected GameObject explosionVFXPrefab;
    [Header("Settings")]
    [SerializeField] protected int moneyValue;
    public static event Action<Enemy> OnAnyEnemyDied;

    protected virtual void Update()
    {
        Move();
    }

    public abstract void Move();
    public abstract void Attack();

    protected void OnEnable()
    {
        health.OnEnemyDied += HandleEnemyDied;
    }

    protected void OnDisable()
    {
        health.OnEnemyDied -= HandleEnemyDied;
    }

    protected virtual void HandleEnemyDied()
    {
        Player.Instance.GetMoneyWallet().AddMoney(moneyValue);
        EnemySpawner.Instance.SetEnemyCount(EnemySpawner.Instance.GetCurrentEnemyCount() - 1);
        GameObject vfx = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
        Destroy(vfx, 2f);
        OnAnyEnemyDied?.Invoke(this);
        Destroy(gameObject);
    }
}
