using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float speed = 1f;
    protected int health = 100;
    private Rigidbody2D rb;
    private int hitScore = 10;
    private Animator animator;
    private bool defeated = false;
    private BoxCollider2D boxCollider;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        rb.linearVelocity = Vector2.left * speed;
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        ScoreManager.Instance.UpdateScore(hitScore);
        if (health <= 0 && !defeated)
        {
            defeated = true;
            OnDefeated();
        }
    }

    private void OnDefeated()
    {
        WaveManager.Instance.OnEnemyDefeated();
        animator.SetTrigger("Defeated");
        rb.linearVelocity = Vector2.zero;
        boxCollider.enabled = false;
    }
}
