using UnityEngine;

public class PiercingArcherHeroTower : Tower
{
    private float baseShootSpeed = 0.5f;
    private int basePierce = 1;
    protected override void Awake()
    {
        base.Awake();
        towerName = "Piercing Archer Hero";

        UpdateShootSpeed(baseShootSpeed);
        UpdatePierce(basePierce);
    }

    protected override void Upgrade1()
    {
        int newDamage = 35;
        UpdateDamage(newDamage);
    }

    protected override void Upgrade2()
    {
        int newPierce = 2;
        UpdatePierce(newPierce);
    }
    protected override void Upgrade3()
    {
        float newShootSpeed = 0.75f;
        UpdateShootSpeed(newShootSpeed);

        int newDamage = 50;
        UpdateDamage(newDamage);
    }

    protected override void Upgrade4()
    {
        int newPierce = 3;
        UpdatePierce(newPierce);
    }
}