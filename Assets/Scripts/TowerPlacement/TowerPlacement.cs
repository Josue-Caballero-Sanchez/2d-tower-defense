using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class TowerPlacement : MonoBehaviour
{
    public static TowerPlacement Instance { get; private set; }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private LayerMask towerLayerMask;
    [SerializeField] private LayerMask placementAreaLayerMask;
    private GameObject towerPrefab;
    private GameObject ghostTower;
    private PlacementArea currentPlacementArea;
    private int towerCost = 0;
    private float placementCooldown = 0.1f;

    private void Awake()
    {
        // Ensure a single instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        HandleGhostTower();
        HandleRightMouseClicked();
        HandleLeftMouseClicked();

        if (placementCooldown > 0)
        {
            placementCooldown -= Time.deltaTime;
        }
    }

    private void HandleRightMouseClicked()
    {
        if (Input.GetMouseButtonDown(1) && IsPlacing())
        {
            CancelPlacement();
        }
    }

    public bool IsPlacing()
    {
        return ghostTower != null;
    }

    private void HandleGhostTower()
    {
        if (ghostTower == null)
        {
            return;
        }

        ghostTower.SetActive(true);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
            out Vector2 localPoint
        );
        ghostTower.GetComponent<RectTransform>().anchoredPosition = localPoint;

        Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, float.MaxValue, placementAreaLayerMask);

        if (hit.collider != null && hit.collider.TryGetComponent(out PlacementArea placementArea))
        {
            currentPlacementArea = placementArea;

            if (!currentPlacementArea.CheckIfHasTowerPlaced())
            {
                // Snap ghost visually to placement area center in screen space
                Vector3 centerWorld = currentPlacementArea.GetComponent<Collider2D>().bounds.center;
                Vector2 centerScreen = mainCamera.WorldToScreenPoint(centerWorld);

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    centerScreen,
                    canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
                    out Vector2 snappedPoint
                );
                ghostTower.GetComponent<RectTransform>().anchoredPosition = snappedPoint;

                if (Input.GetMouseButtonDown(0) && ScoreManager.Instance.GetScore() >= towerCost)
                {
                    PlaceTower(centerWorld);
                }
            }
        }
        else
        {
            currentPlacementArea = null;
        }
    }

    private void PlaceTower(Vector3 position)
    {
        if (currentPlacementArea == null)
        {
            return;
        }

        // Instantiate the tower and assign it to the current placement area
        GameObject tower = Instantiate(towerPrefab, position, Quaternion.identity);
        Tower towerScript = tower.GetComponent<Tower>();
        if (towerScript != null)
        {
            towerScript.SetPlacementArea(currentPlacementArea);
            towerScript.setCamera(mainCamera);
            towerScript.SetPlacementArea(currentPlacementArea);
            towerScript.UpdateTotalCost(towerCost);
        }
        currentPlacementArea.UpdateHasTowerPlaced(true);
        ScoreManager.Instance.UpdateScore(-towerCost);
        placementCooldown = 0.1f;
        CancelPlacement();
    }

    private void HandleLeftMouseClicked()
    {
        if (Input.GetMouseButtonUp(0))
        {

            if (EventSystem.current.IsPointerOverGameObject())
            {
                bool clickedUpgradeUI = false;
                bool clickedInventoryItem = false;
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                if (results.Any(r => r.gameObject.TryGetComponent(out UpgradeUI _)))
                {
                    clickedUpgradeUI = true;
                }
                else if (results.Any(r => r.gameObject.TryGetComponent(out InventorySlot _)))
                {
                    clickedInventoryItem = true;
                }

                if (!clickedInventoryItem && !clickedUpgradeUI)
                {
                    CancelPlacement();
                    UpgradeUI.Instance.Hide();

                }
                else if (!clickedUpgradeUI)
                {
                    UpgradeUI.Instance.Hide();
                }

                return;
            }

            Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D towerHit = Physics2D.Raycast(mouseWorld, Vector2.zero, float.MaxValue, towerLayerMask);
            if (towerHit.collider != null && towerHit.collider.TryGetComponent(out Tower tower))
            {
                CancelPlacement();
                if (placementCooldown <= 0)
                {
                    UpgradeUI.Instance.Show(tower.GetNextUpgrade(), tower);
                }
                return;
            }

            RaycastHit2D placementAreaHit = Physics2D.Raycast(mouseWorld, Vector2.zero, float.MaxValue, placementAreaLayerMask);
            if (placementAreaHit.collider != null && placementAreaHit.collider.TryGetComponent(out PlacementArea placementArea))
            {
                if (placementArea.CheckIfHasTowerPlaced() && IsPlacing())
                {
                    CancelPlacement();
                    UpgradeUI.Instance.Hide();
                    return;
                }
            }

            CancelPlacement();
            UpgradeUI.Instance.Hide();
        }
    }

    public void CancelPlacement()
    {
        if (ghostTower != null)
        {
            Destroy(ghostTower);
            ghostTower = null;
            currentPlacementArea = null;
        }
    }

    public void SetCurrentTower(TowerSO towerSO)
    {
        if (ghostTower != null)
        {
            Destroy(ghostTower);
        }

        towerPrefab = towerSO.towerPrefab;
        towerCost = towerSO.towerCost;

        // Create ghost as UI element inside canvas
        ghostTower = new GameObject("GhostTower");
        ghostTower.transform.SetParent(canvas.transform, false);

        // Calculate correct size for the ghost tower
        float worldUnitsInScreen = mainCamera.orthographicSize * 2f;
        float pixelsPerUnit = Screen.height / worldUnitsInScreen;
        float canvasScale = canvasRect.rect.height / Screen.height;
        float scaledPixelsPerUnit = pixelsPerUnit * canvasScale;

        float towerPrefabScale = 1.25f;

        RectTransform rt = ghostTower.AddComponent<RectTransform>();
        SpriteRenderer originalRenderer = towerPrefab.GetComponentInChildren<SpriteRenderer>();
        rt.sizeDelta = new Vector2(
            originalRenderer.sprite.bounds.size.x * scaledPixelsPerUnit * towerPrefabScale,
            originalRenderer.sprite.bounds.size.y * scaledPixelsPerUnit * towerPrefabScale
        );

        Image ghostImage = ghostTower.AddComponent<Image>();
        ghostImage.sprite = originalRenderer.sprite;
        ghostImage.raycastTarget = false;
        Color ghostColor = originalRenderer.color;
        ghostColor.a = 0.85f;
        ghostImage.color = ghostColor;
    }
}