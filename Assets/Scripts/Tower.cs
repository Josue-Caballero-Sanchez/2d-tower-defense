using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] private LayerMask zombieLayer;
    private PlacementArea placementArea;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        CheckEnemyInLane();
    }

    private void CheckEnemyInLane()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, zombieLayer);
        animator.SetBool("isShooting", hit.collider);
    }

    public virtual void Shoot()
    {
        Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
    }

    public void SetPlacementArea(PlacementArea placementArea)
    {
        this.placementArea = placementArea;
    }
}
