using UnityEngine;

[CreateAssetMenu(fileName = "TowerUpgradeSO", menuName = "Scriptable Objects/TowerUpgradeSO")]
public class TowerUpgradeSO : ScriptableObject
{
    public string towerName;
    public string upgradeName;
    public string description;
    public int level;
    public int upgradeCost;
    public Sprite upgradeIcon;
}
