using UnityEngine;

public class WizardHeroTower : Tower
{
    [SerializeField] private WizardFireballProjectile fireballProjectilePrefab;
    private float baseShootSpeed = 0.6f;
    private float splashRadius = 1.5f;
    private int baseDamage = 25;
    private bool increaseSplashRadius = false;
    protected override void Awake()
    {
        base.Awake();

        towerName = "Wizard Hero";
        UpdateShootSpeed(baseShootSpeed);
        UpdateDamage(baseDamage);
    }

    protected override void Upgrade1()
    {
        float newShootSpeed = 0.8f;
        UpdateShootSpeed(newShootSpeed);
    }

    protected override void Upgrade2()
    {
        baseDamage = 50;
        UpdateDamage(baseDamage);
    }
    protected override void Upgrade3()
    {
        splashRadius = 2f;
        increaseSplashRadius = true;
    }

    protected override void Upgrade4()
    {
        float newShootSpeed = 1.25f;
        UpdateShootSpeed(newShootSpeed);
    }

    public override void Shoot()
    {
        WizardFireballProjectile projectileInstance = Instantiate(fireballProjectilePrefab, shootPoint.position, Quaternion.identity);
        projectileInstance.SetDamage(currentDamage);
        projectileInstance.SetSplashRadius(splashRadius);

        if (increaseSplashRadius)
        {
            projectileInstance.SetIncreaseSplashRadius(true);
        }
    }
}
