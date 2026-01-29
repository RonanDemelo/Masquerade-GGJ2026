using UnityEngine;

/// <summary>
/// Base class for storing health and taking damage
/// </summary>
public class HealthClass : MonoBehaviour
{
    public float maxHealth = 100;
    public float health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        health = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health =- damage;
        }

        if (health <= 0) Death();
    }

    public virtual void Death()
    {
        health = 0;
    }
}
