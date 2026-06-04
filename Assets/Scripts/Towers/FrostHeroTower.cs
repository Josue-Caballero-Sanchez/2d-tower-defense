using UnityEngine;

public class FrostHeroTower : Tower
{
    private int damage = 25;
    private float baseShootSpeed = 1f;
    private float slowAmount = 0.75f;
    private float slowDuration = 0.5f;
    protected override void Awake()
    {
        base.Awake();

        towerName = "Frost Hero";
        UpdateDamage(damage);
        UpdateShootSpeed(baseShootSpeed);
    }

    protected override void Upgrade1()
    {
        damage = 50;
        UpdateDamage(damage);
    }

    protected override void Upgrade2()
    {
        slowAmount = 0.5f;
    }
    protected override void Upgrade3()
    {
        baseShootSpeed = 1.5f;
        UpdateShootSpeed(baseShootSpeed);

        damage = 100;
        UpdateDamage(damage);
    }

    protected override void Upgrade4()
    {
        slowDuration = 1f;
        slowAmount = 0.25f;
    }

    public override void Shoot()
    {
        Projectile projectileInstance = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectileInstance.SetDamage(currentDamage);
        projectileInstance.SetSlowingEffect(slowAmount, slowDuration);
    }
}
