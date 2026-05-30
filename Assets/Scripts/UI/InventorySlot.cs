using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TowerSO towerSO;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI costText;

    private void Awake()
    {
        iconImage.sprite = towerSO.towerIcon;
        costText.text = "$" + towerSO.towerCost.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && TowerPlacement.Instance != null)
        {
            TowerPlacement.Instance.SetCurrentTower(towerSO);
        }
    }
}
