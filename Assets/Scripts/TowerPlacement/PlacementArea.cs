using UnityEngine;

public class PlacementArea : MonoBehaviour
{
    private bool hasTowerPlaced = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D placementCollider;
    [SerializeField] private BoxCollider2D clickCollider;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        DrawOutline();
    }

    public bool CheckIfHasTowerPlaced()
    {
        return hasTowerPlaced;
    }

    public void UpdateHasTowerPlaced(bool newValue)
    {
        hasTowerPlaced = newValue;
    }

    void OnMouseOver()
    {
        if (TowerPlacement.Instance.IsPlacing())
        {
            spriteRenderer.color = Color.green;
        }
    }
    void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
    }

    private void DrawOutline()
    {
        if (TowerPlacement.Instance.IsPlacing() && !hasTowerPlaced)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    public BoxCollider2D GetPlacementCollider()
    {
        return placementCollider;
    }

    public BoxCollider2D GetClickCollider()
    {
        return clickCollider;
    }

}
