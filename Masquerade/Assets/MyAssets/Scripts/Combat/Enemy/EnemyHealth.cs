using UnityEngine;

public class EnemyHealth : HealthClass
{
    protected override void Start()
    {
        base.Start();
    }

    public override void Death()
    {
        base.Death();
        Debug.LogError("Enemy Death Not Implemented");
    }
}
