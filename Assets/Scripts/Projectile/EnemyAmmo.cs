using UnityEngine;

public class EnemyAmmo : Projectile
{
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
                Destroy(gameObject);
            }
        }
    }
}
