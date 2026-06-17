using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TowerSO towerSO;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Image costTextBackgroundImage;
    private Button mainButton;

    private void Awake()
    {
        iconImage.sprite = towerSO.towerIcon;
        costText.text = "$" + towerSO.towerCost.ToString();
        mainButton = GetComponent<Button>();
    }

    private void Update()
    {
        if (ScoreManager.Instance != null)
        {
            bool canAfford = ScoreManager.Instance.GetScore() >= towerSO.towerCost;
            mainButton.interactable = canAfford;
            costTextBackgroundImage.color = canAfford ? new Color32(0, 0, 0, 210) : new Color32(0, 0, 0, 175);
            costText.color = canAfford ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 175);
            iconImage.color = canAfford ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 175);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && TowerPlacement.Instance != null)
        {
            if (ScoreManager.Instance.GetScore() >= towerSO.towerCost)
            {
                TowerPlacement.Instance.SetCurrentTower(towerSO);
            }
        }
    }
}
