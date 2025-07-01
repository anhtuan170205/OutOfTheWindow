using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float lifeTime = 5f;
    [SerializeField] protected int damageAmount = 10;
    [SerializeField] protected LayerMask targetLayer;

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

    public void Launch(Vector3 direction)
    {
        transform.forward = direction.normalized;
    }
}
