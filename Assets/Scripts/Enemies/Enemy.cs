using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int deathScore = 50;
    [SerializeField] protected float speed = 1f;
    [SerializeField] protected float originalSpeed = 1f;
    [SerializeField] protected int health = 100;
    [SerializeField] private bool isEnemyInShield = false;
    [SerializeField] private bool isEnemyEquipment = false;
    protected Rigidbody2D rb;
    private Animator animator;
    private bool defeated = false;
    private BoxCollider2D boxCollider;
    protected Material enemyMaterial;
    private Coroutine hitCoroutine;
    protected Coroutine knockbackCoroutine;
    protected SpriteRenderer spriteRenderer;
    private float slowedSpeed = 0f;
    private float slownessTimer = 0f;
    private float stunTimer = 0f;
    protected bool isStunned = false;
    protected bool isSlowed = false;
    private Camera mainCamera;
    private bool colliderEnabled = false;
    private float spawnTimer = 0f;

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
        boxCollider.enabled = false;
        mainCamera = Camera.main;

        if (isEnemyInShield)
        {
            ApplyShield();
        }
    }

    protected virtual void Update()
    {
        HandleSlow();
        HandleStun();

        if (!colliderEnabled && !defeated)
        {
            CheckIfInsideCamera();
        }

        if (spawnTimer < 1.5f)
        {
            spawnTimer += Time.deltaTime;
        }

        if (colliderEnabled && !defeated && spawnTimer >= 1.5f)
        {
            ClampToCameraBounds();
        }
    }

    private void CheckIfInsideCamera()
    {
        Bounds cameraBounds = GetCameraBounds();
        Bounds enemyBounds = boxCollider.bounds;

        if (cameraBounds.Contains(enemyBounds.min) && cameraBounds.Contains(enemyBounds.max))
        {
            boxCollider.enabled = true;
            colliderEnabled = true;
        }
    }

    private Bounds GetCameraBounds()
    {
        float height = mainCamera.orthographicSize * 2f;
        float width = height * mainCamera.aspect;
        return new Bounds(mainCamera.transform.position, new Vector3(width, height, float.MaxValue));
    }

    private void ClampToCameraBounds()
    {
        if (defeated) return;

        float rightEdge = mainCamera.transform.position.x + (mainCamera.orthographicSize * mainCamera.aspect);
        float halfWidth = boxCollider.bounds.extents.x;

        if (transform.position.x + halfWidth > rightEdge)
        {
            transform.position = new Vector3(rightEdge - halfWidth, transform.position.y, transform.position.z);

            // Stop any velocity pushing it further right
            if (rb.bodyType != RigidbodyType2D.Static && rb.bodyType != RigidbodyType2D.Kinematic)
            {
                if (rb.linearVelocity.x > 0)
                {
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                }
            }
        }
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

    public void ApplyKnockback(float knockbackDistance, float knockbackDuration)
    {
        if (rb == null || defeated || isEnemyEquipment)
        {
            return;
        }

        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
        }
        knockbackCoroutine = StartCoroutine(KnockbackCoroutine(knockbackDistance, knockbackDuration));
    }

    private IEnumerator KnockbackCoroutine(float distance, float duration)
    {
        float elapsed = 0f;
        float startX = transform.position.x;
        float targetX = startX + distance;

        animator.SetFloat("WalkSpeed", 0f);
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            float newX = Mathf.Lerp(startX, targetX, t);
            rb.MovePosition(new Vector2(newX, transform.position.y));
            yield return null;
        }

        if (!defeated)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.left * speed;
            animator.SetFloat("WalkSpeed", speed);
        }

        knockbackCoroutine = null;
    }

    public void ApplyStun(float stunDuration)
    {
        if (rb == null || defeated || isEnemyEquipment)
        {
            return;
        }

        stunTimer = stunDuration;
        isStunned = true;
        speed = 0;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        animator.SetFloat("WalkSpeed", 0f);
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
            animator.SetFloat("WalkSpeed", originalSpeed * slowAmount);
        }

        slowedSpeed = originalSpeed * slowAmount;
        isSlowed = true;
        slownessTimer = duration;
    }

    private void RefreshMovement()
    {
        if (defeated || rb == null)
        {
            return;
        }

        if (!isStunned && knockbackCoroutine == null)
        {
            rb.linearVelocity = Vector2.left * speed;
        }
        animator.SetFloat("WalkSpeed", speed);
    }

    private void HandleSlow()
    {
        if (slownessTimer > 0)
        {
            slownessTimer -= Time.deltaTime;
            spriteRenderer.material.SetFloat("_Glow", 10f);
            spriteRenderer.material.SetColor("_GlowColor", new Color32(0, 203, 255, 255));

            if (slownessTimer <= 0 && spriteRenderer != null)
            {
                isSlowed = false;
                speed = originalSpeed;
                slowedSpeed = 0f;
                spriteRenderer.material.SetFloat("_Glow", 0f);
                spriteRenderer.material.SetColor("_GlowColor", Color.white);
                slownessTimer = 0f;

                if (isStunned)
                {
                    speed = 0f;
                }

                RefreshMovement();
            }
        }
    }

    private void HandleStun()
    {
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            enemyMaterial.SetFloat("_HitEffectGlow", 1f);
            enemyMaterial.SetFloat("_HitEffectBlend", 0.35f);

            if (stunTimer <= 0 && rb != null)
            {
                isStunned = false;
                speed = originalSpeed;
                enemyMaterial.SetFloat("_HitEffectGlow", 1f);
                enemyMaterial.SetFloat("_HitEffectBlend", 0f);
                stunTimer = 0f;

                if (isSlowed)
                {
                    speed = slowedSpeed;
                }

                RefreshMovement();
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
