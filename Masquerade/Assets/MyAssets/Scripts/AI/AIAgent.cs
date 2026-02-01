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

        stateMachine.ChangeState(initialState);

        characterTransform = GameObject.FindGameObjectWithTag("Target").transform;
        characterGameObj = GameObject.FindGameObjectWithTag("Target");
        sensor.distance = config.maxSightDistance;
    }

    private void Update()
    {
        stateMachine.Update();
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    public void OnDeath()
    {
        Destroy(gameObject);
      //  WaveManagement.Instance.EnemyDied();
    }
}
