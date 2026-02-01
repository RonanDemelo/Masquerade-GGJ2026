using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
{
    float timer = 0.0f;

    public void Enter(AIAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = agent.config.maxDistance;
        agent.navMeshAgent.speed = agent.config.runSpeed;
    }

    public void Exit(AIAgent agent)
    {

    }

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Update(AIAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;

        if(!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.characterTransform.position;
        }

        Vector3 _distanceFromPlayer = (agent.characterTransform.position - agent.transform.position);

        if (_distanceFromPlayer.magnitude <= agent.config.attackRange)
        {
            if (!agent.sensor.IsInSight(agent.characterGameObj))
            {
                agent.navMeshAgent.stoppingDistance = 1f;
                agent.navMeshAgent.destination = agent.characterTransform.position;
                return;
            }
            agent.stateMachine.ChangeState(AiStateId.Attack);
        }


        if (timer < 0.0f)
        {
            Vector3 _direction = (agent.characterTransform.position - agent.navMeshAgent.destination);
            _direction.y = 0;

            if(_direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if(agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.characterTransform.position;
                }
            }
            timer = agent.config.maxTime;
        }

    }
}
