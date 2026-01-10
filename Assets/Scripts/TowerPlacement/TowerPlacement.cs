using UnityEngine;
using UnityEngine.XR;

public class TowerPlacement : MonoBehaviour
{
    public static TowerPlacement Instance { get; private set; }

    [SerializeField] private Camera mainCamera;
    private GameObject towerPrefab;
    private GameObject ghostTower;
    private PlacementArea currentPlacementArea;
    private int towerCost = 0;

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
    }

    private void HandleGhostTower()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
        if (hit.collider != null && hit.collider.TryGetComponent(out PlacementArea placementArea))
        {
            currentPlacementArea = placementArea;
            if (!currentPlacementArea.CheckIfHasTowerPlaced() && ghostTower != null)
            {
                // Position the ghost tower at the center of the placement area
                Vector3 centerPosition = currentPlacementArea.GetComponent<Collider2D>().bounds.center;
                centerPosition.z = 0;
                ghostTower.transform.position = centerPosition;

                ghostTower.SetActive(true);

                if (Input.GetMouseButtonDown(0) && ScoreManager.Instance.GetScore() >= towerCost)
                {
                    PlaceTower(centerPosition);
                }
            }
            else
            {
                if (ghostTower != null)
                {
                    ghostTower.SetActive(false);
                }
            }
        }
        else
        {
            if (ghostTower != null)
            {
                ghostTower.SetActive(false);
            }
            currentPlacementArea = null;
        }
    }

    private void PlaceTower(Vector3 position)
    {
        // Instantiate the tower and assign it to the current placement area
        if (currentPlacementArea != null)
        {
            GameObject tower = Instantiate(towerPrefab, position, Quaternion.identity);
            Tower towerScript = tower.GetComponent<Tower>();
            if (towerScript != null)
            {
                towerScript.SetPlacementArea(currentPlacementArea);
            }
            currentPlacementArea.UpdateHasTowerPlaced(true);

            ScoreManager.Instance.UpdateScore(-towerCost);
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

        ghostTower = new GameObject("GhostPlant");
        SpriteRenderer ghostRenderer = ghostTower.AddComponent<SpriteRenderer>();
        SpriteRenderer originalRenderer = towerPrefab.GetComponentInChildren<SpriteRenderer>();
        ghostRenderer.sprite = originalRenderer.sprite;
        Color ghostColor = originalRenderer.color;
        ghostColor.a = 0.7f;
        ghostRenderer.color = ghostColor;
        ghostTower.transform.localScale = towerPrefab.transform.localScale;
    }
}
