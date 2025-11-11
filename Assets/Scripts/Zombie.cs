using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour
{
    [SerializeField] private LayerMask towerLayer;
    private float speed = 1f;
    private float health = 125f;
    private Rigidbody2D rb;
    private int hitScore = 10;

    private void Awake()
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
            Destroy(this.gameObject);
        }
    }
}
