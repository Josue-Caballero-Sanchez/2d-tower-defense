using UnityEngine;

public class ArcherHeroTower : Tower
{
    protected override void Upgrade1()
    {
        Debug.Log("Archer Hero Tower Upgraded to tier 1!");
        int newDamage = 1000000;
        currentDamage = newDamage;
    }
}
