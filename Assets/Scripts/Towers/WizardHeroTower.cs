using UnityEngine;

public class WizardHeroTower : Tower
{
    [SerializeField] private new WizardFireballProjectile projectilePrefab;
    private float baseShootSpeed = 0.6f;
    private float splashRadius = 1.5f;
    protected override void Awake()
    {
        base.Awake();

        towerName = "Wizard Hero";
        UpdateShootSpeed(baseShootSpeed);
    }

    protected override void Upgrade1()
    {
        int newDamage = 35;
        UpdateDamage(newDamage);
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
        int newDamage = 50;
        UpdateDamage(newDamage);

        float newShootSpeed = 1.5f;
        UpdateShootSpeed(newShootSpeed);
    }

    public override void Shoot()
    {
        WizardFireballProjectile projectileInstance = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectileInstance.SetDamage(currentDamage);
        projectileInstance.SetSplashRadius(splashRadius);

        if (currentLevel >= 3)
        {
            projectileInstance.SetIncreaseSplashRadius(true);
        }
    }
}
