using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private TextMeshProUGUI upgradeTitleText;
    [SerializeField] private TextMeshProUGUI upgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI TowerNameText;
    [SerializeField] private TextMeshProUGUI upgradeButtonCostText;
    [SerializeField] private TextMeshProUGUI upgradeButtonText;
    [SerializeField] private Image upgradeButtonIcon;
    [SerializeField] private Image towerIcon;
    [SerializeField] private GameObject upgradeButtonContainer;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Slider upgradeSliderValue;
    [SerializeField] private GameObject showNoUpgrades;
    [SerializeField] private GameObject upgradeDescriptionContainer;
    [SerializeField] private Button SellButton;
    [SerializeField] private TextMeshProUGUI SellCostText;
    [SerializeField] private List<GameObject> upgradeLevelIcons;
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
        SellButton.onClick.AddListener(() => { SellButtonClicked(); });
    }

    private void Update()
    {
        if (currentUpgrade != null)
        {
            if (ScoreManager.Instance.GetScore() < currentUpgrade.upgradeCost)
            {
                DisableUpgradeButton();
            }
            else
            {
                EnableUpgradeButton();
            }
        }
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
            SellCostText.text = "$" + tower.GetSellValue().ToString();
            ShowLevelIcons(tower.GetCurrentLevel());

            upgradeIcon.gameObject.SetActive(false);
            upgradeTitleText.gameObject.SetActive(false);
            upgradeDescriptionContainer.SetActive(false);
            upgradeDescriptionText.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(false);
            upgradeButtonContainer.gameObject.SetActive(false);
            showNoUpgrades.gameObject.SetActive(true);
            return;
        }

        upgradeIcon.gameObject.SetActive(true);
        upgradeTitleText.gameObject.SetActive(true);
        upgradeDescriptionContainer.SetActive(true);
        upgradeDescriptionText.gameObject.SetActive(true);
        upgradeButton.gameObject.SetActive(true);
        upgradeButtonContainer.gameObject.SetActive(true);
        showNoUpgrades.gameObject.SetActive(false);
        ShowLevelIcons(tower.GetCurrentLevel());

        upgradeIcon.sprite = upgrade.upgradeIcon;
        towerIcon.sprite = tower.GetTowerIcon();
        TowerNameText.text = tower.GetTowerName();
        upgradeTitleText.text = upgrade.upgradeName;
        upgradeDescriptionText.text = upgrade.description;
        upgradeButton.gameObject.SetActive(true);
        upgradeButtonCostText.text = "$" + upgrade.upgradeCost.ToString();
        upgradeSliderValue.value = (float)tower.GetCurrentLevel() / 4;
        SellCostText.text = "$" + tower.GetSellValue().ToString();
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
            currentTower.UpdateTotalCost(currentUpgrade.upgradeCost);
            Show(currentTower.GetNextUpgrade(), currentTower);
        }
    }

    public void SellButtonClicked()
    {
        currentTower.Sell();
    }

    private void ShowLevelIcons(int level)
    {
        for (int i = 0; i < upgradeLevelIcons.Count; i++)
        {
            if (i < level)
            {
                upgradeLevelIcons[i].GetComponent<Image>().color = new Color(0.16863f, 0.93725f, 0.49804f);
            }
            else
            {
                upgradeLevelIcons[i].GetComponent<Image>().color = new Color(0.45098f, 0.45098f, 0.45098f);
            }
        }
    }

    private void DisableUpgradeButton()
    {
        upgradeButton.interactable = false;
        upgradeButtonText.color = new Color32(128, 128, 128, 125);
        upgradeButtonCostText.color = new Color32(128, 128, 128, 125);
        upgradeButtonIcon.color = new Color32(255, 255, 255, 125);
    }

    private void EnableUpgradeButton()
    {
        upgradeButton.interactable = true;
        upgradeButtonText.color = new Color32(0, 101, 180, 255);
        upgradeButtonCostText.color = new Color32(0, 101, 180, 255);
        upgradeButtonIcon.color = new Color32(255, 255, 255, 255);
    }
}
