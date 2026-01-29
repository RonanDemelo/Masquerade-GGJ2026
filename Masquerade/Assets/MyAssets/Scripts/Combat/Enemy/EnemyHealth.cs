using UnityEngine;

public class EnemyHealth : HealthClass
{
    public override void Death()
    {
        base.Death();
        Debug.LogError("Enemy Death Not Implemented");
    }
}
