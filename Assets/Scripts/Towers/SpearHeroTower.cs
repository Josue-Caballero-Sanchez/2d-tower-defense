using UnityEngine;

public class SpearHeroTower : Tower
{
    private float baseShootSpeed = 0.25f;
    private int baseDamage = 50;
    private float baseKnockback = 2f;

    protected override void Awake()
    {
        base.Awake();
        towerName = "Spear Hero";

        UpdateShootSpeed(baseShootSpeed);
        UpdateDamage(baseDamage);
    }

    protected override void Upgrade1()
    {
        baseDamage = 100;
        UpdateDamage(baseDamage);
    }

    protected override void Upgrade2()
    {
        baseShootSpeed = 0.75f;
        UpdateShootSpeed(baseShootSpeed);
    }
    protected override void Upgrade3()
    {
        baseKnockback = 3f;
    }

    protected override void Upgrade4()
    {
        baseKnockback = 4f;
        baseShootSpeed = 1f;
        UpdateShootSpeed(baseShootSpeed);
    }

    public override void Shoot()
    {
        Projectile projectileInstance = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectileInstance.SetDamage(currentDamage);
        projectileInstance.SetKnockback(baseKnockback);
    }
}
