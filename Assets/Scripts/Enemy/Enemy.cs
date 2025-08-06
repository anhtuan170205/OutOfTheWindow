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

    private Player player => Player.Instance;
    private TurnManager turnManager => BootstrappedData.Instance.TurnManager;
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
        player.GetMoneyWallet().AddMoney(moneyValue);

        var spawner = turnManager.EnemySpawner;
        spawner.SetEnemyCount(spawner.GetCurrentEnemyCount() - 1);

        GameObject vfx = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
        Destroy(vfx, 2f);

        OnAnyEnemyDied?.Invoke(this);
        Destroy(gameObject);
    }
}
