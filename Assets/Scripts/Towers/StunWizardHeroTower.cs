using UnityEngine;

public class StunWizardHeroTower : Tower
{
    private float baseStunDuration = 0.5f;
    private float baseShootSpeed = 0.5f;
    private int baseDamage = 25;
    protected override void Awake()
    {
        base.Awake();
        towerName = "Stun Wizard Hero";

        UpdateShootSpeed(baseShootSpeed);
        UpdateDamage(baseDamage);
    }

    protected override void Upgrade1()
    {

    }

    protected override void Upgrade2()
    {

    }
    protected override void Upgrade3()
    {

    }

    protected override void Upgrade4()
    {

    }

    public override void Shoot()
    {
        Projectile projectileInstance = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectileInstance.SetDamage(currentDamage);
        projectileInstance.SetStunDuration(baseStunDuration);
    }
}
