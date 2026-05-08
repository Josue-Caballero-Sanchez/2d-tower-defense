using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    protected float speed = 1f;
    protected int health = 100;
    private Rigidbody2D rb;
    private int hitScore = 10;
    private Animator animator;
    private bool defeated = false;
    private BoxCollider2D boxCollider;
    private Material enemyMaterial;
    private Coroutine hitCoroutine;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        enemyMaterial = GetComponentInChildren<SpriteRenderer>().material;
    }
    private void Start()
    {
        rb.linearVelocity = Vector2.left * speed;
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        ScoreManager.Instance.UpdateScore(hitScore);

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

    private IEnumerator HitEffect()
    {
        enemyMaterial.SetColor("_HitEffectColor", Color.red);
        enemyMaterial.SetFloat("_HitEffectGlow", 10f);
        enemyMaterial.SetFloat("_HitEffectBlend", 0.2f);
        yield return new WaitForSeconds(0.1f);

        enemyMaterial.SetColor("_HitEffectColor", Color.clear);
        enemyMaterial.SetFloat("_HitEffectBlend", 0f);
        enemyMaterial.SetFloat("_HitEffectGlow", 0f);
    }

    private void OnDefeated()
    {
        WaveManager.Instance.OnEnemyDefeated();
        animator.SetTrigger("Defeated");
        rb.linearVelocity = Vector2.zero;
        boxCollider.enabled = false;
    }
}
