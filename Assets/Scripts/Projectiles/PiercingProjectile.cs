using Unity.VisualScripting;
using UnityEngine;

public class PiercingProjectile : Projectile
{
    protected override void Awake()
    {
        base.Awake();
        pierce = 2;
    }
}
