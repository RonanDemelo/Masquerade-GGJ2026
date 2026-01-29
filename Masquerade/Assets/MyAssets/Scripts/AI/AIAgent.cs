using UnityEngine;
using UnityEngine.AI;

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


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new AIStateMachine(this);
        ragdoll = GetComponentInChildren<Ragdoll>();
        skinned = GetComponentInChildren<SkinnedMeshRenderer>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        //register states
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIAttackState());

        stateMachine.ChangeState(initialState);

        characterTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
