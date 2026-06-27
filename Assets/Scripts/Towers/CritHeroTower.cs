using UnityEngine;

public class CritHeroTower : Tower
{
    private int baseDamage = 25;
    private float critChance = 0f;
    private int critDamageMultiplier = 2;

    protected override void Awake()
    {
        base.Awake();
        towerName = "Crit Hero";

        UpdateDamage(baseDamage);
    }

    protected override void Upgrade1()
    {
        critChance = 0.25f;
    }

    protected override void Upgrade2()
    {
        baseDamage = 75;
        UpdateDamage(baseDamage);
    }
    protected override void Upgrade3()
    {
        float newShootSpeed = 1.5f;
        UpdateShootSpeed(newShootSpeed);
    }

    protected override void Upgrade4()
    {
        critChance = 0.75f;
    }

    public override void Shoot()
    {
        bool isCrit = critChance > 0 && Random.value < critChance;
        int damage = isCrit ? currentDamage * critDamageMultiplier : currentDamage;

        Projectile projectileInstance = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectileInstance.SetDamage(damage);

        if (isCrit)
        {
            projectileInstance.SetIsCrit(true);
        }
    }
}
