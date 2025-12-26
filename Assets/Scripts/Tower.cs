using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] private LayerMask zombieLayer;
    private float health = 100f;
    private float shootInterval = 1f;
    private float shootTimer = 0f;
    private PlacementArea placementArea;

    private void Update()
    {
        shootTimer += Time.deltaTime;

        if (ZombieInLane() && shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    private bool ZombieInLane()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, zombieLayer);
        return hit.collider != null;
    }

    protected virtual void Shoot()
    {
        Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
    }

    public void SetPlacementArea(PlacementArea placementArea)
    {
        this.placementArea = placementArea;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            placementArea.UpdateHasTowerPlaced(false);
        }
    }
}
