using System.Collections;
using UnityEngine;

public class PlayerAttack : AttackClass
{
    public PlayerCamera cameraClass;

    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float shootForce = 1000f;
    private float nextFireTime;

    protected virtual void Update()
    {
        base.Update();
        //firePoint.transform.LookAt(cameraClass.gameObject.transform.TransformDirection(Vector3.forward));
    }

    public override void MeleeAttack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, cameraClass.gameObject.transform.TransformDirection(Vector3.forward), out hit, meleeRange, layerMask))
        {
            Debug.DrawRay(transform.position, cameraClass.gameObject.transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            hit.collider.GetComponent<EnemyCombat>().health.TakeDamage(damage);
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
        
        Instantiate(maskShard, firePoint.transform.position, Quaternion.LookRotation(transform.right));

        maskShard.damage = damage;
        maskShard.layerMask = layerMask;
        maskShard.shootForce = shootForce;
        maskShard.playerCombat = this.gameObject.GetComponent<PlayerCombat>();
    }
}
