using UnityEngine;

public class Repeater : Tower
{
    protected override void Shoot()
    {
        base.Shoot();
        Invoke(nameof(ShootSecondProjectile), 0.2f);
    }

    private void ShootSecondProjectile()
    {
        Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
    }
}
