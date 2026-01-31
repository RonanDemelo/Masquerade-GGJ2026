using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : AttackClass
{

    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float shootForce = 1000f;
    [SerializeField] private float meleeAttackDuration = 0.15f;
    private float nextFireTime;

    private List<EnemyCombat> enemiesHitThisAttack = new List<EnemyCombat>();
    private bool meleeAttackInProgress = false;
    

    protected virtual void Update()
    {
        base.Update();
        //firePoint.transform.LookAt(cameraClass.gameObject.transform.TransformDirection(Vector3.forward));
    }
    public IEnumerator MeleeAttackRoutine()
    {
        if(meleeAttackInProgress) yield return null;
        meleeAttackInProgress = true;
        enemiesHitThisAttack.Clear();

        float timer = 0f;

        while (timer < meleeAttackDuration)
        {
            MeleeAttack();
            timer += Time.deltaTime;
            yield return null;
        }

        meleeAttackInProgress = false;
    }

    public override void MeleeAttack()
    {

        RaycastHit hit;
        if (Physics.Raycast(firePoint.transform.position, firePoint.gameObject.transform.TransformDirection(Vector3.forward), out hit, meleeRange, layerMask))
        {
            //changedGetComponent to GetComponentInParent- Ronan
            Debug.DrawRay(firePoint.transform.position, firePoint.gameObject.transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue, 10f);
            EnemyCombat enemy = hit.collider.GetComponentInParent<EnemyCombat>();

            if (enemy == null) return;
            if (enemiesHitThisAttack.Contains(enemy)) return;

            enemiesHitThisAttack.Add(enemy);
            enemy.health.TakeDamage(damage);

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
        
        Instantiate(maskShard, firePoint.transform.position, Quaternion.LookRotation(firePoint.transform.forward));

        maskShard.damage = damage;
        maskShard.layerMask = layerMask;
        maskShard.shootForce = shootForce;
        maskShard.playerCombat = this.gameObject.GetComponent<PlayerCombat>();
    }
}
