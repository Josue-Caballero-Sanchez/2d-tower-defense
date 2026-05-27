using UnityEngine;

public class WizardFireballProjectile : Projectile
{
    [SerializeField] private GameObject explosionGameObjectLevelTwo;
    private bool increaseSplashRadius = false;
    private void OnDrawGizmos()
    {
        if (doesSplashDamage)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawSphere(splashPoint.position, splashRadius);
            Gizmos.color = new Color(1f, 0f, 0f, 0.8f);
            Gizmos.DrawWireSphere(splashPoint.position, splashRadius);
        }
    }

    protected override void HandleSplashCollision()
    {
        if (increaseSplashRadius)
        {
            GetComponent<Collider2D>().enabled = false;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
            GetComponentInChildren<SpriteRenderer>().enabled = false;

            explosionGameObjectLevelTwo.SetActive(true);
            StartCoroutine(DestroyProjectile());
        }
        else
        {
            base.HandleSplashCollision();
        }
    }

    public void SetIncreaseSplashRadius(bool value)
    {
        increaseSplashRadius = value;
    }
}
