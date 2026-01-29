using System.Collections;
using UnityEngine;

public class PlayerAttack : AttackClass
{
    public PlayerCamera cameraClass;

    [SerializeField] private float fireRate = 1f;
    private float nextFireTime;


    public override void MeleeAttack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, cameraClass.gameObject.transform.TransformDirection(Vector3.forward), out hit, meleeRange, layerMask))
        {
            //changedGetComponent to GetComponentInParent- Ronan
            Debug.DrawRay(transform.position, cameraClass.gameObject.transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            hit.collider.GetComponentInParent<EnemyCombat>().health.TakeDamage(damage);

            //Ronans AIcode
            var _hitbox = hit.collider.GetComponent<HitBox>();
            if(_hitbox)
            {
                _hitbox.OnRaycastHit(this);
                Debug.Log(_hitbox);
            }
        }
    }

    public IEnumerator ContinueShooting(bool shouldShoot)
    {
        while (shouldShoot)
        {
            RangedAttack();
            yield return new WaitForSeconds(fireRate);
        }
    }

    public override void RangedAttack()
    {
        MaskShard maskShard = projectile.GetComponent<MaskShard>();
        
        Instantiate(maskShard, firePoint.transform.position, transform.rotation);

        maskShard.damage = damage;
        maskShard.layerMask = layerMask;
        maskShard.playerCombat = this.gameObject.GetComponent<PlayerCombat>();
    }
}
