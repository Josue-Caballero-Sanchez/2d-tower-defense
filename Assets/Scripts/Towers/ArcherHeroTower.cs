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
        Debug.Log("Archer Hero Tower Upgraded to tier 1!");
        int newDamage = 35;
        currentDamage = newDamage;
    }

    protected override void Upgrade2()
    {
        Debug.Log("Archer Hero Tower Upgraded to tier 2!");
        float newShootSpeed = 1.4f;
        animator.SetFloat("shootSpeed", newShootSpeed);
    }
}
