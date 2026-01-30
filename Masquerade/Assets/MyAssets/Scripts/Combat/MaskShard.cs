using System;
using System.Collections;
using Unity.VisualScripting;
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
        Shoot();
    }

    private void FixedUpdate()
    {
        //if (direction != Vector3.zero) Shoot();
    }

    public void Shoot()
    {
        rb.linearVelocity = shootForce * transform.forward * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyCombat enemy = other.gameObject.GetComponentInParent<EnemyCombat>();
        if (enemy)
        {
            enemy.health.TakeDamage(damage);
        }
        Debug.Log(other);
        Destroy(gameObject);
    }

    IEnumerator DestroyOnLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
