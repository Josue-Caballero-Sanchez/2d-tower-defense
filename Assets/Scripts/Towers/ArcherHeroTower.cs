using UnityEngine;

public class ArcherHeroTower : Tower
{
    [SerializeField] private Transform shootPoint2;
    private bool isLevel4 = false;
    protected override void Awake()
    {
        base.Awake();
        towerName = "Archer Hero";
    }

    protected override void Upgrade1()
    {
        int newDamage = 35;
        currentDamage = newDamage;
    }

    protected override void Upgrade2()
    {
        float newShootSpeed = 1.4f;
        UpdateShootSpeed(newShootSpeed);
    }
    protected override void Upgrade3()
    {
        float newShootSpeed = 1.8f;
        UpdateShootSpeed(newShootSpeed);

        int newDamage = 50;
        currentDamage = newDamage;
    }

    protected override void Upgrade4()
    {
        isLevel4 = true;
    }

    public override void Shoot()
    {
        if (!isLevel4)
        {
            base.Shoot();
        }
        else
        {
            base.Shoot();
            Projectile projectileInstance = Instantiate(projectilePrefab, shootPoint2.position, Quaternion.identity);
            projectileInstance.SetDamage(currentDamage);
        }
    }
}
