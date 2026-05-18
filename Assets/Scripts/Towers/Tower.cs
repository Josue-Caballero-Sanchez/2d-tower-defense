using UnityEngine;
using System.Collections.Generic;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected Projectile projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask zombieLayer;
    [SerializeField] private List<TowerUpgradeSO> upgrades;
    [SerializeField] private Sprite towerIcon;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Material LevelOneMaterial;
    [SerializeField] private Material LevelTwoMaterial;
    private Camera mainCamera;
    protected string towerName;
    protected int currentDamage = 25;
    protected int currentLevel = 0;
    private int totalCost = 0;
    private PlacementArea placementArea;
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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

    public TowerUpgradeSO GetNextUpgrade()
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.level == currentLevel + 1)
            {
                return upgrade;
            }
        }

        return null;
    }

    public void ApplyUpgrade(int level)
    {
        switch (level)
        {
            case 1:
                Upgrade1();
                spriteRenderer.material = LevelOneMaterial;
                break;
            case 2:
                Upgrade2();
                spriteRenderer.material = LevelTwoMaterial;
                break;
            default:
                break;
        }

        currentLevel++;
    }

    public void UpdateTotalCost(int amount)
    {
        totalCost += amount;
    }

    public int GetSellValue()
    {
        return Mathf.RoundToInt(totalCost * 0.7f);
    }

    public void Sell()
    {
        Destroy(gameObject);
        UpgradeUI.Instance.Hide();
        placementArea.UpdateHasTowerPlaced(false);
        ScoreManager.Instance.UpdateScore(Mathf.RoundToInt(totalCost * 0.7f));
    }
    public void setCamera(Camera camera)
    {
        this.mainCamera = camera;
    }
    public Sprite GetTowerIcon()
    {
        return towerIcon;
    }

    public string GetTowerName()
    {
        return towerName;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    public int GetUpgradesCount()
    {
        return upgrades.Count;
    }
    protected abstract void Upgrade1();
    protected abstract void Upgrade2();
}
