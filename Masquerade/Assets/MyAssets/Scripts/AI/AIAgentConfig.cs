using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    [Header("Basic")]
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float maxSightDistance = 5.0f;
    [Header("Death")]
    public float despawnTime = 5.0f;
    [Header("Attack")]
    public float attackRange = 1.2f;
    public float attackCooldown = 0.5f;
}
