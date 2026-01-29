using System;
using System.Collections;
using UnityEngine;

public class MaskShard : MonoBehaviour
{
    public float lifetime = 1f;

    public PlayerCombat playerCombat;
    [NonSerialized] public LayerMask layerMask;
    [NonSerialized] public float damage;
    public float shootForce;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyOnLifetime());
    }

    private void FixedUpdate()
    {
        Shoot();
    }

    public void Shoot()
    {
        rb.linearVelocity = -1 * shootForce * transform.right * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyCombat enemy = collision.gameObject.GetComponent<EnemyCombat>();
        if (enemy)
        {
            enemy.health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    IEnumerator DestroyOnLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
