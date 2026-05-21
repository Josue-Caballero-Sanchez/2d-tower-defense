using UnityEngine;

public class ArcherHeroTower : Tower
{
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
}
