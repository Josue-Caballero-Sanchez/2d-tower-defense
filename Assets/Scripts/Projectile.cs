using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private float damage = 25f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.linearVelocity = Vector2.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Zombie zombie))
        {
            zombie.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
