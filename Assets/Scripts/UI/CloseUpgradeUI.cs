using UnityEngine;
using UnityEngine.EventSystems;

public class CloseUpgradeUI : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        UpgradeUI.Instance.Hide();
    }
}
