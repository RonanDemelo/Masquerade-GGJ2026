using UnityEngine;

public class EnemyTemp : MonoBehaviour
{
    public float baseHealth = 100f;
    public float baseDamage = 10f;

    float currentHealth;

    public void Initialize(float waveModifier)
    {
        currentHealth = baseHealth * waveModifier;
        baseDamage *= waveModifier;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        //WaveManagement.Instance.EnemyDied();
        Destroy(gameObject);
    }
}
