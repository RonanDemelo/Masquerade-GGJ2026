using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
{
    [Tooltip("insert the Character here not the player empty")]
    public Transform characterTransform;
    float timer = 0.0f;

    public void Enter(AIAgent agent)
    {
        
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
        if(!agent.enabled) return;

        timer -= Time.deltaTime;

        if(!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = characterTransform.position;
        }

        if (timer < 0.0f)
        {
            Vector3 _direction = (characterTransform.position - agent.navMeshAgent.destination);
            _direction.y = 0.0f;
            if(_direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if(agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = characterTransform.position;
                }
            }    
            timer = agent.config.maxTime;
        }
    }
}
