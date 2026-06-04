using UnityEngine;

public class WizardHeroTower : Tower
{
    [SerializeField] private WizardFireballProjectile fireballProjectilePrefab;
    private float baseShootSpeed = 0.6f;
    private float splashRadius = 1.5f;
    private int baseDamage = 50;
    protected override void Awake()
    {
        base.Awake();

        towerName = "Wizard Hero";
        UpdateShootSpeed(baseShootSpeed);
        UpdateDamage(baseDamage);
    }

    protected override void Upgrade1()
    {
        baseDamage = 75;
        UpdateDamage(baseDamage);
    }

    protected override void Upgrade2()
    {
        float newShootSpeed = 0.8f;
        UpdateShootSpeed(newShootSpeed);
    }
    protected override void Upgrade3()
    {
        splashRadius = 2f;
    }

    protected override void Upgrade4()
    {
        baseDamage = 100;
        UpdateDamage(baseDamage);

        float newShootSpeed = 1.5f;
        UpdateShootSpeed(newShootSpeed);
    }

    public override void Shoot()
    {
        WizardFireballProjectile projectileInstance = Instantiate(fireballProjectilePrefab, shootPoint.position, Quaternion.identity);
        projectileInstance.SetDamage(currentDamage);
        projectileInstance.SetSplashRadius(splashRadius);

        if (currentLevel >= 3)
        {
            projectileInstance.SetIncreaseSplashRadius(true);
        }
    }
}
