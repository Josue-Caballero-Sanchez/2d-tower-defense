using Unity.VisualScripting;
using UnityEngine;

public class PlantPlacement : MonoBehaviour
{
    public static PlantPlacement Instance { get; private set; }

    [SerializeField] private Camera mainCamera;
    private GameObject plantPrefab;
    private GameObject ghostPlant;
    private PlacementArea currentPlacementArea;

    private void Awake()
    {
        // Ensure a single instance of the GameManager
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
        if (hit.collider != null && hit.collider.TryGetComponent(out PlacementArea placementArea))
        {
            currentPlacementArea = placementArea;
            if (!currentPlacementArea.CheckIfHasTowerPlaced() && ghostPlant != null)
            {
                // Position the ghost plant at the center of the placement area
                Vector3 centerPosition = currentPlacementArea.GetComponent<Collider2D>().bounds.center;
                centerPosition.z = 0;
                ghostPlant.transform.position = centerPosition;

                ghostPlant.SetActive(true);

                if (Input.GetMouseButtonDown(0) && ScoreManager.Instance.GetScore() >= 50)
                {
                    PlacePlant(centerPosition);
                }
            }
            else
            {
                if (ghostPlant != null)
                {
                    ghostPlant.SetActive(false);
                }
            }
        }
        else
        {
            if (ghostPlant != null)
            {
                ghostPlant.SetActive(false);
            }
            currentPlacementArea = null;
        }
    }

    private void PlacePlant(Vector3 position)
    {
        // Instantiate the plant and assign it to the current placement area
        if (currentPlacementArea != null)
        {
            GameObject tower = Instantiate(plantPrefab, position, Quaternion.identity);
            Tower towerScript = tower.GetComponent<Tower>();
            if (towerScript != null)
            {
                towerScript.SetPlacementArea(currentPlacementArea);
            }
            currentPlacementArea.UpdateHasTowerPlaced(true);

            int plantCost = 50;
            ScoreManager.Instance.UpdateScore(-plantCost);
        }
    }

    public void SetCurrentTower(TowerSO towerSO)
    {
        if (ghostPlant != null)
        {
            Destroy(ghostPlant);
        }

        plantPrefab = towerSO.towerPrefab;

        ghostPlant = new GameObject("GhostPlant");
        SpriteRenderer ghostRenderer = ghostPlant.AddComponent<SpriteRenderer>();
        SpriteRenderer originalRenderer = plantPrefab.GetComponent<SpriteRenderer>();
        ghostRenderer.sprite = originalRenderer.sprite;
        Color ghostColor = originalRenderer.color;
        ghostColor.a = 0.7f;
        ghostRenderer.color = ghostColor;
        ghostPlant.transform.localScale = plantPrefab.transform.localScale;
    }
}
