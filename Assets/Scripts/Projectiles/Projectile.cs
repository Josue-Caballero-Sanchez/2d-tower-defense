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
    [SerializeField] private float knockback = 0;
    private int pierce = 0;
    private int damage = 0;
    protected Rigidbody2D rb;

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
            if (knockback > 0)
            {
                enemy.ApplyKnockback(knockback);
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

    public void SetKnockback(float newKnockback)
    {
        knockback = newKnockback;
    }

    public void SetSlowingEffect(float newSlowAmount, float newSlowDuration)
    {
        slowsEnemy = true;
        slowAmount = newSlowAmount;
        slowDuration = newSlowDuration;
    }
}
