using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected Health health;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] protected GameObject explosionVFXPrefab;

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
        EnemySpawner.Instance.SetEnemyCount(EnemySpawner.Instance.GetCurrentEnemyCount() - 1);
        GameObject vfx = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
        Destroy(vfx, 2f);
        Destroy(gameObject);
    }

    public Health GetHealth()
    {
        return health;
    }
}
