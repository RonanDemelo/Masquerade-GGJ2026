using System;
using System.Collections;
using UnityEngine;

public class MaskShard : MonoBehaviour
{
    public float lifetime = 1f;

    public PlayerCombat playerCombat;
    public LayerMask layerMask;
    public float damage;
    [NonSerialized]public float shootForce = 50000f;
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
        Debug.Log(collision.collider.transform);
        EnemyCombat enemy = collision.gameObject.GetComponentInParent<EnemyCombat>();
        if (enemy)
        {
            Debug.Log($"Bullet damage: {damage}");
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
