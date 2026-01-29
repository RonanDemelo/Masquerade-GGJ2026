using UnityEngine;

public enum AiStateId
{
    ChasePlayer,
    Death,
    Idle
}
public interface AIState
{
    AiStateId GetId();
    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent);
}
