using UnityEngine;

/// <summary>
/// Base class for storing health and taking damage
/// </summary>
public class HealthClass : MonoBehaviour
{
    public float baseHealth = 100;
    public float currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        currentHealth = baseHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth =- damage;
        }

        if (currentHealth <= 0) Death();
    }

    public virtual void Death()
    {
        currentHealth = 0;
    }
}
