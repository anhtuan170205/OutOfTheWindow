using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private LayerMask targetLayer;

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
        }
    }

    public void Launch(Vector3 direction)
    {
        transform.forward = direction.normalized;
    }
}
