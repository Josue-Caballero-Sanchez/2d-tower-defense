using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private LayerMask towerLayer;
    private float speed = 1f;
    protected float health = 125f;
    private Rigidbody2D rb;
    private int hitScore = 10;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        rb.linearVelocity = Vector2.left * speed;
    }

    public void TakeDamage(float damage)
    {
        health = health - damage;
        ScoreManager.Instance.UpdateScore(hitScore);
        if (health <= 0)
        {
            OnDefeated();
        }
    }

    private void OnDefeated()
    {
        WaveManager.Instance.OnEnemyDefeated();
        Destroy(this.gameObject);
    }
}
