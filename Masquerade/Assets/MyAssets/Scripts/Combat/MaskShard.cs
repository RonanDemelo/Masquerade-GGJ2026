using System;
using UnityEngine;

public class MaskShard : MonoBehaviour
{
    public PlayerCombat playerCombat;
    [NonSerialized] public LayerMask layerMask;
    [NonSerialized] public float damage;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Shoot();
    }

    public void Shoot()
    {
        rb.linearVelocity = new Vector3(10f * Time.deltaTime, 0, 0);
    }
}
