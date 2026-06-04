using UnityEngine;

public class RageEnemy : Enemy
{
    private float baseSpeed;
    private float maxHealthAtStart;

    protected override void Start()
    {
        base.Start();
        baseSpeed = speed;
        originalSpeed = baseSpeed;
        maxHealthAtStart = Mathf.Max(1f, health);
    }
    protected override void Update()
    {
        base.Update();

        if (knockbackCoroutine == null && !isStunned && !isSlowed)
        {
            float healthRatio = Mathf.Clamp01((maxHealthAtStart - health) / maxHealthAtStart);
            float targetSpeed = Mathf.Lerp(baseSpeed, 3f, healthRatio);
            speed = Mathf.Max(baseSpeed, targetSpeed);
            originalSpeed = speed;
            rb.linearVelocity = Vector2.left * speed;
        }

    }
}
