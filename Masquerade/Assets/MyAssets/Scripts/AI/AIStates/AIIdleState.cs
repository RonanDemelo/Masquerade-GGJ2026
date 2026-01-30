using UnityEngine;

public class AIIdleState : AIState
{
    public void Enter(AIAgent agent)
    {
        
    }

    public void Exit(AIAgent agent)
    {
        
    }

    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }

    public void Update(AIAgent agent)
    {
        Vector3 _playerDirection = agent.characterTransform.position - agent.transform.position;
        if(_playerDirection.magnitude > agent.config.maxSightDistance)
        {
            return;
        }

        Vector3 _agentDirection = agent.transform.forward;
        _playerDirection.Normalize();
        float _dotProduct = Vector3.Dot(_playerDirection, _agentDirection);

        if( _dotProduct > 0 )
        {
            //agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
            agent.stateMachine.ChangeState(AiStateId.Attack);
        }
    }
}
