using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private TextMeshProUGUI upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI TowerNameText;
    [SerializeField] private TextMeshProUGUI upgradeButtonText;
    [SerializeField] private Image towerIcon;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI noUpgradesText;
    [SerializeField] private GameObject upgradesCount0;
    [SerializeField] private GameObject upgradesCount1;
    [SerializeField] private GameObject upgradesCount2;
    private Tower currentTower;
    private TowerUpgradeSO currentUpgrade;
    private void Awake()
    {
        // Ensure a single instance of the ScoreManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Hide();
    }

    public void Show(TowerUpgradeSO upgrade, Tower tower)
    {
        gameObject.SetActive(true);
        currentTower = tower;
        currentUpgrade = upgrade;

        upgradesCount0.SetActive(false);
        upgradesCount1.SetActive(false);
        upgradesCount2.SetActive(false);
        switch (tower.GetCurrentTier())
        {
            case 0:
                upgradesCount0.SetActive(true);
                break;
            case 1:
                upgradesCount1.SetActive(true);
                break;
            case 2:
                upgradesCount2.SetActive(true);
                break;
            default:
                break;
        }

        if (upgrade == null)
        {
            TowerNameText.text = tower.GetTowerName();
            towerIcon.sprite = tower.GetTowerIcon();
            upgradeButton.gameObject.SetActive(false);
            noUpgradesText.gameObject.SetActive(true);
            return;
        }

        noUpgradesText.gameObject.SetActive(false);
        upgradeIcon.sprite = upgrade.upgradeIcon;
        towerIcon.sprite = tower.GetTowerIcon();
        TowerNameText.text = tower.GetTowerName();
        upgradeDescriptionText.text = upgrade.description;
        upgradeButton.gameObject.SetActive(true);
        upgradeButtonText.text = upgrade.upgradeName + " $" + upgrade.upgradeCost.ToString();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpgradeButtonClicked()
    {
        if (ScoreManager.Instance.GetScore() >= currentUpgrade.upgradeCost)
        {
            ScoreManager.Instance.UpdateScore(-currentUpgrade.upgradeCost);
            currentTower.ApplyUpgrade(currentUpgrade.tier);
            Show(currentTower.GetNextUpgrade(), currentTower);
        }
    }
}
