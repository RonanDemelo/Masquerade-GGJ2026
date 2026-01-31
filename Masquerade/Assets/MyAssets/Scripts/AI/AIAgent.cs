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
    public EnemyAttack enemyAttack;
    public EnemyCombat enemyHealth;
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
        enemyHealth = GetComponentInChildren<EnemyCombat>();
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
