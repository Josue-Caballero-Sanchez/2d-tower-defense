using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeTowerButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UpgradeUI upgradeUI;
    public void OnPointerClick(PointerEventData eventData)
    {
        upgradeUI.UpgradeButtonClicked();
    }
}
