using UnityEngine;
using System.Collections;

public class EnemyShield : Enemy
{
    [SerializeField] private Enemy enemyInShield;


    protected override void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyMaterial = spriteRenderer.material;
    }

    protected override void Start()
    {

    }

    protected override void Update()
    {

    }

    protected override IEnumerator HitEffect()
    {
        enemyMaterial.SetFloat("_HitEffectGlow", 1f);
        enemyMaterial.SetFloat("_HitEffectBlend", 0.6f);
        yield return new WaitForSeconds(0.15f);

        enemyMaterial.SetFloat("_HitEffectGlow", 1f);
        enemyMaterial.SetFloat("_HitEffectBlend", 0f);
    }

    protected override void OnDefeated()
    {
        enemyInShield.disableShield();
        Destroy(gameObject);
    }
}
