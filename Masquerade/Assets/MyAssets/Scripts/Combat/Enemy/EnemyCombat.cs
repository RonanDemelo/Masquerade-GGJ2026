using UnityEngine;

public class EnemyCombat : CombatCharacter
{
    public void SetSpawnHealth(float waveModifier)
    {
        health.baseHealth = health.currentHealth * waveModifier;
        health.currentHealth = health.baseHealth;
    }
}
