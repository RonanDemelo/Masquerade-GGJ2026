using UnityEngine;

public class PlayerHealth : HealthClass
{
    public override void Death()
    {
        base.Death();
        Debug.LogError("Player Death Not Implemented");
    }
}
