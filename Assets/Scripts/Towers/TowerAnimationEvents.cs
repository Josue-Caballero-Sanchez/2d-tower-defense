using UnityEngine;

public class TowerAnimationEvents : MonoBehaviour
{
    private Tower tower;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void Shoot()
    {
        tower.Shoot();
    }

    private void SetShootToFalse()
    {
        tower.SetAnimatorShootToFalse();
    }
}
