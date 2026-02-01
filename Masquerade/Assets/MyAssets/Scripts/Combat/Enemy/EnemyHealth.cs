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
    [SerializeField] private GameObject shatteringMask;
    [SerializeField] private GameObject mask;
    [SerializeField] AudioClip DieSFX;



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
        ShatterMask();
        base.Death();
        AIDeathState _deathState = agent.stateMachine.GetState(AiStateId.Death) as AIDeathState;
        agent.stateMachine.ChangeState(AiStateId.Death);
        WaveManagement.Instance.EnemyDied();
        moneyValue *= (int)AccoladeTracker.Instance.shardModifier;
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

    private void ShatterMask()
    {
        Transform newParent = gameObject.transform.parent;
        GameObject newMask = Instantiate(shatteringMask, newParent);
        newMask.transform.position = mask.transform.position;
        SoundManager.instance.PlaySound3D(DieSFX, transform.position, 0.9f, Random.Range(0.95f, 1.15f));
        Destroy(mask);
    }
}
