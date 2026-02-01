using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatrolState : AIState
{
    Vector3 randomDirection;
    Vector3 finalPosition;
    public void Enter(AIAgent agent)
    {
        randomDirection = Random.insideUnitSphere * agent.config.walkRadius;
        agent.navMeshAgent.speed = agent.config.walkSpeed;
    }

    public void Exit(AIAgent agent)
    {
    }

    public AiStateId GetId()
    {
        return AiStateId.Patrol;
    }

    public void Update(AIAgent agent)
    {

        if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(agent.transform.position, agent.config.walkRadius, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.navMeshAgent.SetDestination(point);
            }
        }
        return;
        
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
