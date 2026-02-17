using UnityEngine;

public class TierTwoSlime : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        health = 200;
    }
}
