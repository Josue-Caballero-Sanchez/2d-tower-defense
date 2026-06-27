using UnityEngine;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using System.Collections;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected Projectile projectilePrefab;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private List<TowerUpgradeSO> upgrades;
    [SerializeField] private Sprite towerIcon;
    [SerializeField] private Material LevelOneMaterial;
    [SerializeField] private Material LevelTwoMaterial;
    [SerializeField] private Material LevelThreeMaterial;
    [SerializeField] private Material LevelFourMaterial;
    [SerializeField] private GameObject auraGameObject;
    [SerializeField] private MMFeedbacks upgradeFeedback;
    [SerializeField] private MMFeedbacks finalUpgradeFeedback;
    [SerializeField] private MMFeedbacks placeFeedback;
    [SerializeField] private MMFeedbacks sellFeedback;
    [SerializeField] private GameObject selectionIndicator;
    private SpriteRenderer spriteRenderer;
    protected string towerName;
    protected int currentDamage = 25;
    protected int currentLevel = 0;
    private int totalCost = 0;
    private float shootSpeed = 1f;
    private PlacementArea placementArea;
    protected Animator animator;
    private float levelTwoAnimationtimer = 0f;
    private float levelTwoAnimationDuration = 1f;
    private int animationDirection = 1;
    protected bool isLevelTwoAnimationActive = false;
    private bool isTowerCurrentlySelected = false;
    private int pierce = 0;
    protected bool isSold = false;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        UpdateShootSpeed(shootSpeed);
        placeFeedback.PlayFeedbacks();
    }

    protected virtual void Update()
    {
        CheckEnemyInLane();
        if (isLevelTwoAnimationActive)
        {
            LevelTwoAnimation();
        }
    }

    protected void LevelTwoAnimation()
    {
        levelTwoAnimationtimer += Time.deltaTime * animationDirection;

        if (levelTwoAnimationtimer > levelTwoAnimationDuration)
        {
            levelTwoAnimationtimer = levelTwoAnimationDuration;
            animationDirection = -1;
        }
        if (levelTwoAnimationtimer < 0)
        {
            animationDirection = 1;
            levelTwoAnimationtimer = 0;
        }
        spriteRenderer.material.SetFloat("_OutlineAlpha", levelTwoAnimationtimer / levelTwoAnimationDuration);
    }

    private void CheckEnemyInLane()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, enemyLayer);
        animator.SetBool("isShooting", hit.collider);
    }

    public virtual void Shoot()
    {
        Projectile projectileInstance = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectileInstance.SetDamage(currentDamage);
        projectileInstance.SetPierce(pierce);
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
                isLevelTwoAnimationActive = true;
                break;
            case 3:
                Upgrade3();
                spriteRenderer.material = LevelThreeMaterial;
                break;
            case 4:
                Upgrade4();
                spriteRenderer.material = LevelFourMaterial;
                auraGameObject.SetActive(true);
                break;
            default:
                break;
        }

        currentLevel++;
        if (currentLevel == upgrades.Count)
        {
            finalUpgradeFeedback.PlayFeedbacks();
        }
        else
        {
            upgradeFeedback.PlayFeedbacks();
        }

        if (isTowerCurrentlySelected)
        {
            ShowSelectionIndicator();
        }
    }

    public void ResetMaterial(int level)
    {
        switch (level)
        {
            case 1:
                spriteRenderer.material = LevelOneMaterial;
                break;
            case 2:
                spriteRenderer.material = LevelTwoMaterial;
                isLevelTwoAnimationActive = true;
                break;
            case 3:
                spriteRenderer.material = LevelThreeMaterial;
                break;
            case 4:
                spriteRenderer.material = LevelFourMaterial;
                auraGameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void UpdateTotalCost(int amount)
    {
        totalCost += amount;
    }

    public int GetSellValue()
    {
        return Mathf.RoundToInt(totalCost * 0.7f);
    }

    protected void UpdateShootSpeed(float newShootSpeed)
    {
        shootSpeed = newShootSpeed;
        animator.SetFloat("shootSpeed", newShootSpeed);
    }

    protected void UpdateDamage(int newDamage)
    {
        currentDamage = newDamage;
    }

    public void UpdatePierce(int newPierce)
    {
        pierce = newPierce;
    }

    public void Sell()
    {
        isSold = true;
        sellFeedback.PlayFeedbacks();
        UpgradeUI.Instance.HideAll();
        placementArea.UpdateHasTowerPlaced(false);
        ScoreManager.Instance.UpdateScore(Mathf.RoundToInt(totalCost * 0.7f));
        StartCoroutine(DestroyAfterFeedback());
    }
    private IEnumerator DestroyAfterFeedback()
    {
        spriteRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        animator.enabled = false;
        Destroy(auraGameObject);

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void ShowSelectionIndicator()
    {
        //selectionIndicator.SetActive(true);
        isTowerCurrentlySelected = true;

        spriteRenderer.material.SetColor("_OutlineColor", Color.white);
        spriteRenderer.material.SetFloat("_OutlineAlpha", 1f);
        spriteRenderer.material.SetFloat("_OutlineGlow", 1.5f);
        spriteRenderer.material.SetFloat("_OutlineWidth", 0.015f);
        if (currentLevel == 2)
        {
            isLevelTwoAnimationActive = false;
        }
    }

    public void HideSelectionIndicator()
    {
        //selectionIndicator.SetActive(false);
        isTowerCurrentlySelected = false;

        spriteRenderer.material.SetFloat("_OutlineAlpha", 0f);
        ResetMaterial(currentLevel);
    }

    public void SetAnimatorShootToFalse()
    {
        animator.SetBool("isShooting", false);
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
    protected abstract void Upgrade3();
    protected abstract void Upgrade4();
}