using UnityEngine;

public class AIAttackState : AIState
{
    float timer = 0.0f;
    public void Enter(AIAgent agent)
    {
        timer = agent.config.attackCooldown;
    }

    public void Exit(AIAgent agent)
    {
        
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
            agent.enemyAttack.MeleeAttack();
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
    }
}
