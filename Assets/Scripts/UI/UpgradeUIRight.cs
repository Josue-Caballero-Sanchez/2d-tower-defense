public class UpgradeUIRight : UpgradeUI
{
    protected override void Awake()
    {
        Hide();
    }

    public void ShowRight(TowerUpgradeSO upgrade, Tower tower)
    {
        gameObject.SetActive(true);
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

    public override void UpgradeButtonClicked()
    {
        if (ScoreManager.Instance.GetScore() >= currentUpgrade.upgradeCost)
        {
            ScoreManager.Instance.UpdateScore(-currentUpgrade.upgradeCost);
            currentTower.ApplyUpgrade(currentUpgrade.level);
            currentTower.UpdateTotalCost(currentUpgrade.upgradeCost);
            ShowRight(currentTower.GetNextUpgrade(), currentTower);
        }
    }

    public void HideRight()
    {
        gameObject.SetActive(false);
    }
}
