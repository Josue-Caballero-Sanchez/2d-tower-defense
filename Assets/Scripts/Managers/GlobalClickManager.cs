using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class GlobalClickManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClicks();
        }
    }

    private void HandleClicks()
    {
        // Check if clicked on UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            TowerPlacement.Instance.CancelPlacement();
            return;
        }


        // Check if clicked a tower
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.GetComponent<Tower>() != null)
        {
            TowerPlacement.Instance.CancelPlacement();
            return;
        }

        UpgradeUI.Instance.Hide();
        TowerPlacement.Instance.CancelPlacement();
    }
}

