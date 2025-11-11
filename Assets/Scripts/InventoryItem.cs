using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private TowerSO towerSO;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Start()
    {
        image.sprite = towerSO.towerIcon;
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
