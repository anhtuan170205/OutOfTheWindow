using UnityEngine;

public class Drone : Enemy
{
    [Header("Explosion")]
    [SerializeField] private float chargeDistance = 5f;
    [SerializeField] private float chargeSpeed = 20f;
    [SerializeField] private float chargeCooldown = 5f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int explosionDamage = 50;

    private bool isCharging;
    private float chargeTimer;
    public override void Move()
    {
        if (isCharging)
        {
            ChargeTowardsPlayer();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        if (distanceToPlayer <= chargeDistance && chargeTimer <= 0f)
        {
            StartCharge();
        }
        else
        {
            navMeshAgent.SetDestination(Player.Instance.transform.position);
        }
    }

    private void StartCharge()
    {
        isCharging = true;
        navMeshAgent.enabled = false;
        chargeTimer = chargeCooldown;
    }

    private void ChargeTowardsPlayer()
    {
        Vector3 direction = (Player.Instance.transform.position - transform.position).normalized;
        transform.position += direction * chargeSpeed * Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        if (distanceToPlayer <= explosionRadius / 2)
        {
            StopCharge();
            Attack();
        }
    }

    private void StopCharge()
    {
        isCharging = false;
        navMeshAgent.enabled = true;
    }

    protected override void Update()
    {
        base.Update();

        if (isCharging)
        {
            chargeTimer -= Time.deltaTime;
        }
    }

    public override void Attack()
    {
        Explode();
    }

    private void Explode()
    {
        health.TakeDamage(health.GetCurrentHealth());
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            Player player = hitCollider.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(explosionDamage);
            }
        }
    }
}
