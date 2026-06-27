using UnityEngine;
using System.Collections;

public class EnemyShield : Enemy
{
    [SerializeField] private Enemy enemyInShield;
    private CircleCollider2D circleCollider2D;


    protected override void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyMaterial = spriteRenderer.material;
        circleCollider2D = GetComponentInChildren<CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();
    }

    protected override void Start()
    {

    }

    protected override void Update()
    {

    }

    protected override IEnumerator HitEffect()
    {
        /*
        enemyMaterial.SetFloat("_HitEffectGlow", 1f);
        enemyMaterial.SetFloat("_HitEffectBlend", 0.6f);
        yield return new WaitForSeconds(0.15f);

        enemyMaterial.SetFloat("_HitEffectGlow", 1f);
        enemyMaterial.SetFloat("_HitEffectBlend", 0f);
        */
        animator.SetTrigger("IsHit");

        yield return new WaitForSeconds(0f);
    }

    protected override void OnDefeated()
    {
        enemyInShield.disableShield();
        animator.SetTrigger("Destroy");
        circleCollider2D.enabled = false;
    }

    public void DisableCollider()
    {
        circleCollider2D.enabled = false;
    }

    public void EnableCollider()
    {
        circleCollider2D.enabled = true;
    }
}