using UnityEditor;
using UnityEngine;

public class EnemyAttack : AttackClass
{
    [SerializeField] private float shootForce = 1000f;

    public override void MeleeAttack()
    {
        RaycastHit hit;
        if (Physics.SphereCast(firePoint.transform.position, 1f ,firePoint.gameObject.transform.forward, out hit, meleeRange, layerMask))
        {
            Debug.Log(hit.collider.tag);
            Debug.DrawRay(firePoint.transform.position, firePoint.gameObject.transform.forward, color:Color.blue, 5.0f, true);
            if(hit.collider.tag == "Player")
            {
                hit.collider.GetComponent<PlayerCombat>().health.TakeDamage(damage);
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
            MeleeAttack();
        }
        if(attackType == AttackType.Ranged)
        {
            RangedAttack();
        }
    }
}
