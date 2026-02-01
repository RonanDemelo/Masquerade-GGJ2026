using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public AIAgentConfig config;
    public Ragdoll ragdoll;
    public SkinnedMeshRenderer skinned;
    public Transform characterTransform;
    public GameObject characterGameObj;
    public EnemyAttack enemyAttack;
    public EnemyCombat enemyCombat;
    public WeaponIK weaponIK;
    public AISensor sensor;
    Animator animator;


    public float timer = 0f;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        weaponIK = GetComponent<WeaponIK>();
        stateMachine = new AIStateMachine(this);
        ragdoll = GetComponentInChildren<Ragdoll>();
        skinned = GetComponentInChildren<SkinnedMeshRenderer>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        enemyCombat = GetComponentInChildren<EnemyCombat>();
        animator = GetComponentInChildren<Animator>();
        sensor = GetComponent<AISensor>();
        //register states
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIAttackState());
        stateMachine.RegisterState(new AIPatrolState());
        stateMachine.RegisterState(new AISlowChaseState());

        stateMachine.ChangeState(initialState);

        characterTransform = GameObject.FindGameObjectWithTag("Target").transform;
        characterGameObj = GameObject.FindGameObjectWithTag("Target");
        sensor.distance = config.maxSightDistance;

        timer = config.lastSeenTimer;

        if(enemyCombat.maskColour == characterGameObj.GetComponentInParent<PlayerCombat>().maskColour)
        {
            stateMachine.ChangeState(AiStateId.Patrol);
        }
    }

    private void Update()
    {
        stateMachine.Update();
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

        timer -= Time.deltaTime;

        if (sensor.IsInSight(characterGameObj))
        {
            timer = config.lastSeenTimer;
        }
        if(timer < 0)
        {
            Debug.Log("Respawning");
            this.transform.position = WaveManagement.Instance.spawnPoints[Random.Range(0,5)].position;
            timer = config.lastSeenTimer;
        }
    }

    public void OnDeath()
    {
        Destroy(gameObject);
      //  WaveManagement.Instance.EnemyDied();
    }
}
