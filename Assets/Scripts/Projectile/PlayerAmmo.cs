using UnityEngine;

public class PlayerAmmo : Projectile
{
    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
