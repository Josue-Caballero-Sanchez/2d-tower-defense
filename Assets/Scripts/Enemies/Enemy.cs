using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int deathScore = 50;
    [SerializeField] protected float speed = 1f;
    [SerializeField] private float originalSpeed = 1f;
    [SerializeField] protected int health = 100;
    [SerializeField] private bool isEnemyInShield = false;
    [SerializeField] private bool isEnemyEquipment = false;
    private Rigidbody2D rb;
    private Animator animator;
    private bool defeated = false;
    private BoxCollider2D boxCollider;
    protected Material enemyMaterial;
    private Coroutine hitCoroutine;
    private Coroutine knockbackCoroutine;
    private SpriteRenderer spriteRenderer;
    private float slownessTimer = 0f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        enemyMaterial = GetComponentInChildren<SpriteRenderer>().material;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    protected virtual void Start()
    {
        rb.linearVelocity = Vector2.left * speed;

        if (isEnemyInShield)
        {
            ApplyShield();
        }
    }

    protected virtual void Update()
    {
        HandleSlow();
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
        }

        hitCoroutine = StartCoroutine(HitEffect());
        if (health <= 0 && !defeated)
        {
            defeated = true;
            OnDefeated();
        }
    }

    public void ApplyKnockback(float knockbackAmount)
    {
        if (rb == null || defeated || isEnemyEquipment)
        {
            return;
        }

        rb.AddForce(Vector2.right * knockbackAmount, ForceMode2D.Impulse);

        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
        }
        float knockbackDelay = 0.3f;
        knockbackCoroutine = StartCoroutine(RestoreMovementAfterKnockback(knockbackDelay));
    }

    private IEnumerator RestoreMovementAfterKnockback(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (rb != null && !defeated)
        {
            rb.linearVelocity = Vector2.left * speed;
        }
        knockbackCoroutine = null;
    }

    public virtual void ApplySlow(float slowAmount, float duration)
    {
        if (isEnemyEquipment)
        {
            return;
        }

        if (speed > (originalSpeed * slowAmount) && rb != null)
        {
            speed = originalSpeed * slowAmount;
            rb.linearVelocity = Vector2.left * speed;
        }

        slownessTimer = duration;
        spriteRenderer.material.SetFloat("_Glow", 10f);
        spriteRenderer.material.SetColor("_GlowColor", new Color32(0, 203, 255, 255));
        animator.SetFloat("WalkSpeed", originalSpeed * slowAmount);
    }

    private void HandleSlow()
    {
        if (slownessTimer > 0)
        {
            slownessTimer -= Time.deltaTime;
            if (slownessTimer <= 0 && spriteRenderer != null)
            {
                speed = originalSpeed;
                rb.linearVelocity = Vector2.left * speed;
                spriteRenderer.material.SetFloat("_Glow", 0f);
                spriteRenderer.material.SetColor("_GlowColor", Color.white);
                animator.SetFloat("WalkSpeed", originalSpeed);

                slownessTimer = 0f;
            }
        }
    }

    protected virtual IEnumerator HitEffect()
    {
        enemyMaterial.SetFloat("_HitEffectGlow", 1f);
        enemyMaterial.SetFloat("_HitEffectBlend", 0.35f);
        yield return new WaitForSeconds(0.15f);

        enemyMaterial.SetFloat("_HitEffectGlow", 1f);
        enemyMaterial.SetFloat("_HitEffectBlend", 0f);
    }

    protected virtual void OnDefeated()
    {
        WaveManager.Instance.OnEnemyDefeated();
        ScoreManager.Instance.UpdateScore(deathScore);

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static;
        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
            knockbackCoroutine = null;
        }

        animator.SetTrigger("Defeated");
        boxCollider.enabled = false;
    }

    public void ApplyShield()
    {
        boxCollider.enabled = false;
    }

    public void disableShield()
    {
        boxCollider.enabled = true;
    }
}
