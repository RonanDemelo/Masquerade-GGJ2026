using UnityEngine;

/// <summary>
/// Base class for storing health and taking damage
/// </summary>
public class HealthClass : MonoBehaviour
{
    public float baseHealth = 100;
    public float currentHealth;
    public bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        currentHealth = baseHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;
        if (currentHealth > 0)
        {
            currentHealth =- damage;
        }
    }

    public virtual void Death()
    {
        isDead = true;
        currentHealth = 0;
    }

    protected virtual void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }

    }
}
