using UnityEngine;

public class EnemyCombat : CombatCharacter
{
    public void SetSpawnHealth(float waveModifier)
    {
        health.maxHealth = health.health * waveModifier;
        health.health = health.maxHealth;
    }
}
