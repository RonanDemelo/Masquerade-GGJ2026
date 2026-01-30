using System;
using System.Collections;
using UnityEngine;

public class AIBullet : MonoBehaviour
{
    public float lifetime = 1f;

    public EnemyCombat enemyCombat;
    public LayerMask layerMask;
    [NonSerialized] public float damage;
    [NonSerialized] public float shootForce = 5000f;
    [NonSerialized] public Vector3 direction;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Shoot();
        StartCoroutine(DestroyOnLifetime());
    }

    public void Shoot()
    {
        rb.linearVelocity = shootForce * transform.forward * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerCombat enemy = collision.gameObject.GetComponentInParent<PlayerCombat>();
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
