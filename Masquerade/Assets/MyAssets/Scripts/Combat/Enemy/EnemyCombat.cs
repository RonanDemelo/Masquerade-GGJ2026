using UnityEngine;

public class EnemyCombat : CombatCharacter
{
    [Header("Firendly")]
    public MaskType.MaskColour maskColour;
    public void SetSpawnHealth(float waveModifier)
    {
        health.baseHealth = health.currentHealth * waveModifier;
        health.currentHealth = health.baseHealth;
    }  
}
