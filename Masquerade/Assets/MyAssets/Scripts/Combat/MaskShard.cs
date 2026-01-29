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
    [NonSerialized] public Vector3 direction;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyOnLifetime());
    }

    private void FixedUpdate()
    {
        if (direction != Vector3.zero) Shoot();
    }

    public void Shoot()
    {
        rb.linearVelocity = shootForce * transform.forward * Time.deltaTime;
        Debug.Log(rb.linearVelocity);
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
