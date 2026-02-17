using UnityEngine;

public class FastSlime : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        health = 150;
        speed = 2f;
    }
}
