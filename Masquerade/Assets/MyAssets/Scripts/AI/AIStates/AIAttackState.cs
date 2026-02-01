using UnityEngine;

public class AIAttackState : AIState
{
    float timer = 0.0f;
    public void Enter(AIAgent agent)
    {
        timer = agent.config.attackCooldown;
        if(agent.enemyAttack.attackType == AttackClass.AttackType.Ranged)
        {
            agent.weaponIK.targetTransform = agent.characterTransform.transform;
        }
    }

    public void Exit(AIAgent agent)
    {
        agent.weaponIK.targetTransform = null;
    }

    public AiStateId GetId()
    {
        return AiStateId.Attack;
    }

    public void Update(AIAgent agent)
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            if (agent.navMeshAgent.velocity.magnitude == 0)
            {
                if (agent.sensor.IsInSight(agent.characterGameObj))
                {
                    agent.enemyAttack.AttackPlayer();
                }
                else
                {
                    agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
                }

            }
            

            Vector3 _distanceFromPlayer = (agent.characterTransform.position - agent.transform.position);
            if (_distanceFromPlayer.magnitude > agent.config.attackRange)
            {
                agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
            }
            timer = agent.config.attackCooldown;
        }
    }
}
