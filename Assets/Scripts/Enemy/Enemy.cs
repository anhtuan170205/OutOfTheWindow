using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected Health health;
    [SerializeField] protected float damage;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected NavMeshAgent navMeshAgent;

    protected virtual void Update()
    {
        Move();
        Attack();
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
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }

    public Health GetHealth()
    {
        return health;
    }
}
