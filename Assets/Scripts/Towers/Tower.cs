using UnityEngine;
using System.Collections.Generic;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected Projectile projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask zombieLayer;
    [SerializeField] private List<TowerUpgradeSO> upgrades;
    [SerializeField] private Sprite towerIcon;
    protected string towerName;
    protected int currentDamage = 25;
    private int currentTier = 0;
    private PlacementArea placementArea;
    protected Animator animator;

    protected virtual void Awake()
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
        Projectile projectileInstance = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectileInstance.SetDamage(currentDamage);
    }

    public void SetPlacementArea(PlacementArea placementArea)
    {
        this.placementArea = placementArea;
    }

    private void OnMouseUp()
    {
        UpgradeUI.Instance.Show(GetNextUpgrade(), this);
    }

    public TowerUpgradeSO GetNextUpgrade()
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.tier == currentTier + 1)
            {
                return upgrade;
            }
        }

        return null;
    }

    public void ApplyUpgrade(int tier)
    {
        switch (tier)
        {
            case 1:
                Upgrade1();
                break;
            case 2:
                Upgrade2();
                break;
            default:
                break;
        }

        currentTier++;
    }
    public Sprite GetTowerIcon()
    {
        return towerIcon;
    }

    public string GetTowerName()
    {
        return towerName;
    }

    public int GetCurrentTier()
    {
        return currentTier;
    }
    protected abstract void Upgrade1();
    protected abstract void Upgrade2();
}
