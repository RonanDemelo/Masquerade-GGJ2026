using UnityEngine;

public class EnemyHealth : HealthClass
{
    [SerializeField]Ragdoll ragdoll;
    AIAgent agent;

    public SkinnedMeshRenderer skinned;
    public float blinkIntensitity;
    public float blinkDuration;
    private float blinkTimer;



    public void Initialize(float _waveModifier)
    {
        baseHealth = baseHealth * _waveModifier;
        EnemyAttack enemyAttack = GetComponent<EnemyAttack>();
        if (enemyAttack != null)
        {
            enemyAttack.damage = enemyAttack.damage * _waveModifier;
        }
      //  baseDamage *= waveModifier;
    }
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<AIAgent>();


        ragdoll = GetComponentInChildren<Ragdoll>();
        skinned = GetComponentInChildren<SkinnedMeshRenderer>();

        var _rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidBody in _rigidBodies)
        {
            HitBox hitbox = rigidBody.gameObject.AddComponent<HitBox>();
            hitbox.enemyHealth = this;
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        blinkTimer = blinkDuration;

    }
    public override void Death()
    {
        base.Death();
        AIDeathState _deathState = agent.stateMachine.GetState(AiStateId.Death) as AIDeathState;
        agent.stateMachine.ChangeState(AiStateId.Death);
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float _lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float _intensity = (_lerp * blinkIntensitity) + 1.0f;
        skinned.material.color = Color.white * _intensity;
    }
}
