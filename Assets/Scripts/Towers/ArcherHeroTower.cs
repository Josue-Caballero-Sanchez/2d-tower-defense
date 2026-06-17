using UnityEngine;

public class ArcherHeroTower : Tower
{
    [SerializeField] private Transform shootPoint2;
    [SerializeField] private Transform shootPoint3;
    private int baseDamage = 25;
    private bool isLevel2 = false;
    private bool isLevel4 = false;
    protected override void Awake()
    {
        base.Awake();
        towerName = "Archer Hero";

        UpdateDamage(baseDamage);
    }

    protected override void Upgrade1()
    {
        int newDamage = 34;
        UpdateDamage(newDamage);
    }

    protected override void Upgrade2()
    {
        isLevel2 = true;
    }
    protected override void Upgrade3()
    {
        float newShootSpeed = 1.5f;
        UpdateShootSpeed(newShootSpeed);
    }

    protected override void Upgrade4()
    {
        isLevel4 = true;
        isLevel2 = false;
    }

    public override void Shoot()
    {
        if (isLevel2)
        {
            base.Shoot();
            Projectile projectileInstance = Instantiate(projectilePrefab, shootPoint2.position, Quaternion.identity);
            projectileInstance.SetDamage(currentDamage);
        }
        else if (isLevel4)
        {
            base.Shoot();
            Projectile projectileInstance = Instantiate(projectilePrefab, shootPoint2.position, Quaternion.identity);
            projectileInstance.SetDamage(currentDamage);

            Projectile projectileInstance2 = Instantiate(projectilePrefab, shootPoint3.position, Quaternion.identity);
            projectileInstance2.SetDamage(currentDamage);
        }
        else
        {
            base.Shoot();
        }
    }
}