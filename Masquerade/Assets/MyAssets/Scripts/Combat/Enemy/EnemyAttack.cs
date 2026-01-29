using UnityEditor;
using UnityEngine;

public class EnemyAttack : AttackClass
{
    public override void MeleeAttack()
    {
        RaycastHit hit;
        if (Physics.SphereCast(firePoint.transform.position, 0.5f ,firePoint.gameObject.transform.forward, out hit, meleeRange, layerMask))
        {
            Debug.Log(hit.collider.GetComponent<GameObject>());
            Debug.DrawRay(firePoint.transform.position, firePoint.gameObject.transform.forward, color:Color.blue, 5.0f);
            hit.collider.GetComponent<PlayerCombat>().health.TakeDamage(damage);
        }
    }
}
