using UnityEngine;

public class PiercingArcherHeroTower : Tower
{
    private float baseShootSpeed = 0.5f;
    protected override void Awake()
    {
        base.Awake();
        towerName = "Piercing Archer Hero";

        UpdateShootSpeed(baseShootSpeed);
    }

    protected override void Upgrade1()
    {

    }

    protected override void Upgrade2()
    {

    }
}
