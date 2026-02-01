using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerAttack : AttackClass
{

    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float reloadSpeed = 1f;
    public GameObject gunHolder;
    public bool canShoot = true;
    public int currentClip = 10;
    public int maxClipSize = 10;
    public TMP_Text clipText;
    [SerializeField] private float shootForce = 1000f;
    [SerializeField] private float meleeAttackDuration = 0.15f;
    [SerializeField] private GameObject playerGunHolder;
    [SerializeField] LayerMask aimLayer;
    [SerializeField] Camera cam;

    private float nextFireTime;

    private List<EnemyCombat> enemiesHitThisAttack = new List<EnemyCombat>();
    private bool meleeAttackInProgress = false;

    public AudioClip rangedAttackSound;
    public float rangedAttackVloume = 0.5f;

    private float gunTimer;
    

    protected virtual void Update()
    {
        base.Update();
        gunTimer -= Time.deltaTime;
        clipText.text = $"{currentClip}";
    }

    private void FixedUpdate()
    {
        PointGun();
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
           // if (!canShoot) shouldShoot = false;
            if (currentClip <= 0)
            {
                currentClip = 0;
                if (AccoladeTracker.Instance.money <= 0) shouldShoot = false;
                Reload();
                yield return null;
            }
            else
            {
                RangedAttack();
                yield return new WaitForSeconds(fireRate);
            }
        }
    }
    public void Reload()
    {
        if (!canShoot) return;
        canShoot = false;
        Animator gunAnimator = gunHolder.GetComponent<Animator>();
        gunAnimator.speed = reloadSpeed;
        gunAnimator.SetBool("isReload", true);
    }
    public override void RangedAttack()
    {
        if (Time.timeScale == 0) return;
        if(gunTimer > 0) { return; }
        MaskShard maskShard = projectile.GetComponent<MaskShard>();
        
        Instantiate(maskShard, firePoint.transform.position, Quaternion.LookRotation(firePoint.transform.forward));
        currentClip--;
        //play sound
        SoundManager.instance.PlaySound2D(rangedAttackSound, rangedAttackVloume, 1);

        maskShard.damage = damage;
        maskShard.layerMask = layerMask;
        maskShard.shootForce = shootForce;
        maskShard.playerCombat = this.gameObject.GetComponent<PlayerCombat>();
        gunTimer = fireRate;

    }

    private void PointGun()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f, aimLayer))
        {
            Vector3 targetPos = hit.point;
            Vector3 direction = targetPos - playerGunHolder.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerGunHolder.transform.rotation = Quaternion.Slerp(playerGunHolder.transform.rotation, targetRotation, Time.deltaTime * 10f); // rotation speed);
        }
    }

    public void ChangeFireRate(float _change)
    {
        fireRate += _change;
    }
    public void ChangeReloadSpeed(float _change)
    {
        reloadSpeed += _change;
    }

}
