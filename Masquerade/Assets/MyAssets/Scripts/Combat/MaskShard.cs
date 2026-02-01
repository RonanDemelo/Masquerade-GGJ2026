using System;
using System.Collections;
using UnityEngine;

public class MaskShard : MonoBehaviour
{
    public float lifetime = 1f;

    public PlayerCombat playerCombat;
    public LayerMask layerMask;
    public float damage;
    public float shootForce = 50000f;
    [NonSerialized] public Vector3 direction;
    [SerializeField] AudioClip bulletHitEnemy;
    [SerializeField] AudioClip bulletHitOther;

    private Rigidbody rb;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
      //  Physics.IgnoreCollision(playerCombat.GetComponentInParent<Collider>(), GetComponent<Collider>());
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
            SoundManager.instance.PlaySound3D(bulletHitEnemy, transform.position, 0.65f, 1);
        }
        SoundManager.instance.PlaySound3D(bulletHitOther, transform.position, 1, 1);
        Destroy(gameObject);

    }

    IEnumerator DestroyOnLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
