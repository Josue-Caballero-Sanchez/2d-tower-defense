using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private TowerSO towerSO;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI costText;

    public void Awake()
    {
        iconImage.sprite = towerSO.towerIcon;
        costText.text = towerSO.towerCost.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlantPlacement.Instance != null)
        {
            PlantPlacement.Instance.SetCurrentTower(towerSO);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
