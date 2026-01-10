using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 9f;
    private int damage = 0;
    private Rigidbody2D rb;
    protected int pierce = 0;
    protected bool infinitePierce = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.linearVelocity = Vector2.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy zombie))
        {
            zombie.TakeDamage(damage);
            if (pierce > 0 || infinitePierce)
            {
                pierce--;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
}
