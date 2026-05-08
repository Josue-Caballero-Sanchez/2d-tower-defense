using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private TextMeshProUGUI upgradeTitleText;
    [SerializeField] private TextMeshProUGUI upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI TowerNameText;
    [SerializeField] private TextMeshProUGUI upgradeButtonText;
    [SerializeField] private Image towerIcon;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Slider upgradeSliderValue;
    [SerializeField] private GameObject showNoUpgrades;
    [SerializeField] private GameObject upgradeDescriptionContainer;
    private Tower currentTower;
    private TowerUpgradeSO currentUpgrade;
    private void Awake()
    {
        // Ensure a single instance
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

    private void Start()
    {
        upgradeButton.onClick.AddListener(() => { UpgradeButtonClicked(); });
        closeButton.onClick.AddListener(() => { UpgradeUI.Instance.Hide(); });
    }

    public void Show(TowerUpgradeSO upgrade, Tower tower)
    {
        gameObject.SetActive(true);
        currentTower = tower;
        currentUpgrade = upgrade;

        if (upgrade == null)
        {
            TowerNameText.text = tower.GetTowerName();
            towerIcon.sprite = tower.GetTowerIcon();
            upgradeSliderValue.value = (float)tower.GetCurrentLevel() / 4;

            upgradeIcon.gameObject.SetActive(false);
            upgradeTitleText.gameObject.SetActive(false);
            upgradeDescriptionContainer.SetActive(false);
            upgradeDescriptionText.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(false);
            showNoUpgrades.gameObject.SetActive(true);
            return;
        }

        upgradeIcon.gameObject.SetActive(true);
        upgradeTitleText.gameObject.SetActive(true);
        upgradeDescriptionContainer.SetActive(true);
        upgradeDescriptionText.gameObject.SetActive(true);
        upgradeButton.gameObject.SetActive(true);
        showNoUpgrades.gameObject.SetActive(false);

        upgradeIcon.sprite = upgrade.upgradeIcon;
        towerIcon.sprite = tower.GetTowerIcon();
        TowerNameText.text = tower.GetTowerName();
        upgradeTitleText.text = upgrade.upgradeName;
        upgradeDescriptionText.text = upgrade.description;
        upgradeButton.gameObject.SetActive(true);
        upgradeButtonText.text = "$" + upgrade.upgradeCost.ToString();
        upgradeSliderValue.value = (float)tower.GetCurrentLevel() / 4;
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
            currentTower.ApplyUpgrade(currentUpgrade.level);
            Show(currentTower.GetNextUpgrade(), currentTower);
        }
    }
}
