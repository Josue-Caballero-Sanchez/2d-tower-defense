using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] private MMFeedbacks destroyFeedback;
    [SerializeField] private MMFeedbacks hitFeedback;
    [SerializeField] private float speed = 9f;
    [SerializeField] protected bool doesSplashDamage = false;
    [SerializeField] protected float splashRadius = 0;
    [SerializeField] protected bool infinitePierce = false;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] protected Transform splashPoint;
    [SerializeField] protected GameObject explosionGameObject;
    [SerializeField] private bool slowsEnemy = false;
    [SerializeField] private float slowAmount = 0;
    [SerializeField] private float slowDuration = 0;
    [SerializeField] private float knockbackDistance = 0;
    [SerializeField] private float knockbackDuration = 0.1f;
    [SerializeField] private float stunDuration = 0;
    private int pierce = 0;
    private int damage = 0;
    protected Rigidbody2D rb;
    private bool hasCollided = false;

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
        if (collision.TryGetComponent(out Enemy enemy))
        {
            if (hasCollided)
            {
                return;
            }

            if (pierce <= 0 && !infinitePierce)
            {
                hasCollided = true;
            }
            if (doesSplashDamage)
            {
                HandleSplashDamage();
                return;
            }

            enemy.TakeDamage(damage);
            hitFeedback.PlayFeedbacks();

            if (slowsEnemy)
            {
                enemy.ApplySlow(slowAmount, slowDuration);
            }
            if (knockbackDistance > 0)
            {
                enemy.ApplyKnockback(knockbackDistance, knockbackDuration);
            }
            if (stunDuration > 0)
            {
                enemy.ApplyStun(stunDuration);
            }

            if (pierce > 0 || infinitePierce)
            {
                pierce--;
            }
            else
            {
                DestroyProjectileAfterFeedbacks();
            }
        }
    }

    public void HandleSplashDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(splashPoint.position, splashRadius, enemyLayer);
        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
            }
        }

        HandleSplashCollision();
    }

    protected virtual void HandleSplashCollision()
    {
        GetComponent<Collider2D>().enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponentInChildren<SpriteRenderer>().enabled = false;

        explosionGameObject.SetActive(true);
        StartCoroutine(DestroyProjectile());
    }

    public void DestroyProjectileAfterFeedbacks()
    {
        GetComponent<Collider2D>().enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        destroyFeedback.PlayFeedbacks();
        StartCoroutine(DestroyProjectile());
    }

    public IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public void SetPierce(int newPierce)
    {
        pierce = newPierce;
    }

    public void SetSplashRadius(float newRadius)
    {
        splashRadius = newRadius;
    }

    public void SetKnockback(float newKnockbackDistance, float newKnockbackDuration)
    {
        knockbackDistance = newKnockbackDistance;
        knockbackDuration = newKnockbackDuration;
    }

    public void SetStunDuration(float newStunDuration)
    {
        stunDuration = newStunDuration;
    }

    public void SetSlowingEffect(float newSlowAmount, float newSlowDuration)
    {
        slowsEnemy = true;
        slowAmount = newSlowAmount;
        slowDuration = newSlowDuration;
    }
}
