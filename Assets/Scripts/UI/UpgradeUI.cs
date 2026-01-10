using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }
    private Image upgradeIcon;
    private TextMeshProUGUI upgradeNameText;
    private TextMeshProUGUI upgradeDescriptionText;
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

        upgradeIcon = GetComponentsInChildren<Image>()[1];
        upgradeNameText = GetComponentInChildren<TextMeshProUGUI>();
        upgradeDescriptionText = GetComponentsInChildren<TextMeshProUGUI>()[1];
        Hide();
    }

    public void Show(TowerUpgradeSO upgrade, Tower tower)
    {
        gameObject.SetActive(true);
        currentTower = tower;
        currentUpgrade = upgrade;

        upgradeIcon.sprite = upgrade.upgradeIcon;
        upgradeNameText.text = upgrade.upgradeName;
        upgradeDescriptionText.text = upgrade.description;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpgradeButtonClicked()
    {
        currentTower.ApplyUpgrade(currentUpgrade.tier);
    }
}
