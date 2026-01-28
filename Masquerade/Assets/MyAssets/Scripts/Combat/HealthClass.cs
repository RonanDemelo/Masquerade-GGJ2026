using UnityEngine;

public class HealthClass : MonoBehaviour
{
    public float maxHealth = 100;
    private float health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        // quick test code
        //if (Input.GetKeyDown(KeyCode.Backspace))
        //{
        //    TakeDamage(10);
        //    Debug.Log("Damage Test");
        //}
    }

    public virtual void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
        }

        if (health <= 0) Death();
    }

    public virtual void Death()
    {
        health = 0;
    }
}
