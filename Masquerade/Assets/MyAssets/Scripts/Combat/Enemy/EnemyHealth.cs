using UnityEngine;

public class EnemyHealth : HealthClass
{
    [SerializeField]Ragdoll ragdoll;
    AIAgent agent;

    public SkinnedMeshRenderer skinned;
    public float blinkIntensitity;
    public float blinkDuration;
    private float blinkTimer;
    public int scoreValue;
    public int moneyValue; 




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
        
        //agent.stateMachine.ChangeState(AiStateId.ChasePlayer);

    }
    public override void Death()
    {
        if (isDead) return;
        base.Death();
        AIDeathState _deathState = agent.stateMachine.GetState(AiStateId.Death) as AIDeathState;
        agent.stateMachine.ChangeState(AiStateId.Death);
        WaveManagement.Instance.EnemyDied();
        AccoladeTracker.Instance.IncreaseScore(scoreValue);
        AccoladeTracker.Instance.ChangeMoney(moneyValue);
    }

    protected override void Update()
    {
        base.Update();
        if (skinned)
        {
            blinkTimer -= Time.deltaTime;
            float _lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
            float _intensity = (_lerp * blinkIntensitity) + 1.0f;
            skinned.material.color = Color.white * _intensity;
        }
    }
}
