using UnityEngine;

public class Drone : Enemy
{
    public override void Move()
    {
        navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    public override void Attack()
    {

    }
}
