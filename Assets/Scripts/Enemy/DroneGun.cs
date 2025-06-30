using UnityEngine;
using System.Collections.Generic;

public class DroneGun : Enemy
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject enemyAmmoPrefab;
    [SerializeField] private List<Transform> firePointList;
    [SerializeField] private Transform droneTransform;
    private float fireTimer = 0f;
    public override void Move()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        if (distanceToPlayer <= detectionRange)
        {
            navMeshAgent.isStopped = true;
            Attack();
        }
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(Player.Instance.transform.position);
        }
    }

    public override void Attack()
    {
        Aim();
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            FireProjectiles();
            fireTimer = fireRate;
        }
    }

    private void Aim()
    {
        Vector3 targetPosition = Player.Instance.transform.position + Vector3.up * 1.0f;
        Vector3 direction = (targetPosition - droneTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        droneTransform.rotation = Quaternion.Slerp(droneTransform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void FireProjectiles()
    {
        foreach (Transform firePoint in firePointList)
        {
            GameObject ammo = Instantiate(enemyAmmoPrefab, firePoint.position, firePoint.rotation);
            Projectile projectile = ammo.GetComponent<Projectile>();
            if (projectile != null)
            {
                Vector3 targetPosition = Player.Instance.transform.position + Vector3.up * 1.0f;
                Vector3 direction = (targetPosition - firePoint.position).normalized;
                projectile.Launch(direction);
            }
        }
    }
}
