using Unity.AppUI.UI;
using UnityEngine;

public class AIDeathState : AIState
{
    public void Enter(AIAgent agent)
    {
        Vector3 _dir = new Vector3(0, 2, 0);
        agent.ragdoll.ActivateRagdoll();
        agent.ragdoll.ApplyForce(_dir);
        agent.skinned.updateWhenOffscreen = false;

    }

    public void Exit(AIAgent agent)
    {
        
    }

    public AiStateId GetId()
    {
        return AiStateId.Death;
    }

    public void Update(AIAgent agent)
    {
        
    }
}
