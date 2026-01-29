using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public AIAgentConfig config;
    //public Ragdoll ragdoll;


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new AIStateMachine(this);

        //register states
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIDeathState());

        stateMachine.ChangeState(initialState);
        
    }

    private void Update()
    {
        stateMachine.Update();
    }
}
