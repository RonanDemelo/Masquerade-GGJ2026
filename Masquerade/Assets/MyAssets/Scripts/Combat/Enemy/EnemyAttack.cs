using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class EnemyAttack : AttackClass
{
    [SerializeField] private float shootForce = 1000f;
    WeaponIK weaponIK;
    //List
    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Start()
    {
        weaponIK = GetComponent<WeaponIK>();
        weaponIK.aimTransform = firePoint.transform;
    }

    public override void MeleeAttack()
    {
        RaycastHit hit;
        Debug.Log("I'm trying bro");
        if (Physics.SphereCast(firePoint.transform.position, 1f , firePoint.gameObject.transform.forward,
            out hit, meleeRange, layerMask))
        {

            Debug.Log(hit.collider.tag);
            Debug.DrawRay(firePoint.transform.position, firePoint.gameObject.transform.forward, color:Color.blue, 5.0f, true);
            if(hit.collider.tag == "Player")
            {
                Debug.Log($"damage: {damage}");
                hit.collider.GetComponent<PlayerCombat>().health.TakeDamage(damage);
            }
        }
    }

    public void NewMelee()
    {
        Collider[] hits = Physics.OverlapSphere(firePoint.transform.position, 1f,layerMask);

        foreach (var col in hits)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log($"damage: {damage}");
                col.GetComponent<PlayerCombat>()
                   ?.health.TakeDamage(damage);
            }
        }
    }

    public override void RangedAttack()
    {
        AIBullet maskShard = projectile.GetComponent<AIBullet>();

        Instantiate(maskShard, firePoint.transform.position, Quaternion.LookRotation(firePoint.transform.forward));

        maskShard.damage = damage;
        maskShard.layerMask = layerMask;
        maskShard.shootForce = shootForce;
        maskShard.enemyCombat = this.gameObject.GetComponent<EnemyCombat>();
    }

    public void AttackPlayer()
    {
        if(attackType == AttackType.Melee)
        {
            NewMelee();
        }
        if(attackType == AttackType.Ranged)
        {
            RangedAttack();
        }
    }
}
