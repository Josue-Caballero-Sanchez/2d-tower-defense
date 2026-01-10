using UnityEngine;

[CreateAssetMenu(fileName = "TowerUpgradeSO", menuName = "Scriptable Objects/TowerUpgradeSO")]
public class TowerUpgradeSO : ScriptableObject
{
    public string towerName;
    public string upgradeName;
    public string description;
    public int tier;
    public int upgradeCost;
    public Sprite upgradeIcon;
}
