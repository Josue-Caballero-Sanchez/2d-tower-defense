using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using MoreMountains.Feedbacks;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }
    [SerializeField] protected Image upgradeIcon;
    [SerializeField] protected TextMeshProUGUI upgradeTitleText;
    [SerializeField] protected TextMeshProUGUI upgradeDescriptionText;
    [SerializeField] protected TextMeshProUGUI TowerNameText;
    [SerializeField] protected TextMeshProUGUI upgradeButtonCostText;
    [SerializeField] private TextMeshProUGUI upgradeButtonText;
    [SerializeField] private Image upgradeButtonIcon;
    [SerializeField] protected Image towerIcon;
    [SerializeField] protected GameObject upgradeButtonContainer;
    [SerializeField] protected Button upgradeButton;
    [SerializeField] protected Button closeButton;
    [SerializeField] protected GameObject showNoUpgrades;
    [SerializeField] protected GameObject upgradeDescriptionContainer;
    [SerializeField] protected Button SellButton;
    [SerializeField] protected TextMeshProUGUI SellCostText;
    [SerializeField] private List<GameObject> upgradeLevelIcons;
    [SerializeField] private UpgradeUIRight rightPopup;
    [SerializeField] protected MMFeedbacks openFeedback;
    [SerializeField] protected MMFeedbacks closeFeedback;
    protected Tower currentTower;
    protected TowerUpgradeSO currentUpgrade;
    protected virtual void Awake()
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
        gameObject.SetActive(false);
    }

    private void Start()
    {
        upgradeButton.onClick.AddListener(() => { UpgradeButtonClicked(); });
        closeButton.onClick.AddListener(() => { Hide(); });
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

    public void ShowPopup(TowerUpgradeSO upgrade, Tower tower, bool useRightPopup)
    {
        if (useRightPopup)
        {
            Hide();
            rightPopup.ShowRight(upgrade, tower);
        }
        else
        {
            Instance.rightPopup.HideRight();
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
                openFeedback.PlayFeedbacks();
            }
            Instance.Show(upgrade, tower);
        }
    }

    public void HideAll()
    {
        //Instance.gameObject.SetActive(false);
        closeFeedback.PlayFeedbacks();
        Instance.rightPopup.HideRight();
        if (currentTower != null)
        {
            currentTower.HideSelectionIndicator();
        }
    }

    public virtual void Show(TowerUpgradeSO upgrade, Tower tower, bool showRightPopup = false)
    {
        currentTower = tower;
        currentUpgrade = upgrade;

        if (upgrade == null)
        {
            TowerNameText.text = tower.GetTowerName();
            towerIcon.sprite = tower.GetTowerIcon();
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
        SellCostText.text = "$" + tower.GetSellValue().ToString();
    }

    public virtual void Hide()
    {
        closeFeedback.PlayFeedbacks();
        if (currentTower != null)
        {
            currentTower.HideSelectionIndicator();
        }
    }

    public virtual void UpgradeButtonClicked()
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

    protected void ShowLevelIcons(int level)
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
        upgradeButtonText.color = new Color32(128, 128, 128, 175);
        upgradeButtonCostText.color = new Color32(128, 128, 128, 175);
        upgradeButtonIcon.color = new Color32(255, 255, 255, 175);
    }

    private void EnableUpgradeButton()
    {
        upgradeButton.interactable = true;
        upgradeButtonText.color = new Color32(0, 101, 180, 255);
        upgradeButtonCostText.color = new Color32(0, 101, 180, 255);
        upgradeButtonIcon.color = new Color32(255, 255, 255, 255);
    }
}