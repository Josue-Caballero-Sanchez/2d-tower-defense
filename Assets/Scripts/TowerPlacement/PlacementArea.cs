using UnityEngine;

public class PlacementArea : MonoBehaviour
{
    private bool hasTowerPlaced = false;

    public bool CheckIfHasTowerPlaced()
    {
        return hasTowerPlaced;
    }

    public void UpdateHasTowerPlaced(bool newValue)
    {
        hasTowerPlaced = newValue;
    }
}
