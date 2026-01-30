using System.Collections;
using UnityEngine;

public class PlayerAttack : AttackClass
{
    public GameObject playerCamera;

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
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.gameObject.transform.TransformDirection(Vector3.forward), out hit, meleeRange, layerMask))
        {
            //changedGetComponent to GetComponentInParent- Ronan
            Debug.DrawRay(playerCamera.transform.position, playerCamera.gameObject.transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue, 10f);
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
        RangedAttack();
        while (shouldShoot)
        {
            yield return new WaitForSeconds(fireRate);
            RangedAttack();
        }
    }

    public override void RangedAttack()
    {
        MaskShard maskShard = projectile.GetComponent<MaskShard>();
        maskShard.damage = damage;
        maskShard.layerMask = layerMask;
        maskShard.shootForce = shootForce;
        maskShard.playerCombat = this.gameObject.GetComponent<PlayerCombat>();

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(1000f);

        Vector3 direction = (targetPoint - firePoint.transform.position).normalized;

        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 5f, Color.red, 1f);
        Debug.DrawRay(firePoint.transform.position, firePoint.transform.forward * 5f, Color.green, 1f);


        Instantiate(maskShard, firePoint.transform.position, Quaternion.LookRotation(direction));

    }
}
