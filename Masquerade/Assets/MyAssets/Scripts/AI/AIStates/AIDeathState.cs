using Unity.AppUI.UI;
using UnityEngine;

public class AIDeathState : AIState
{
    public void Enter(AIAgent agent)
    {
        //ragdoll.ActivateRagdoll();
        //direction.y = 1;
        //ragdoll.ApplyForce(direction * dieForce);
        //healthBar.Gameobject.SetActive(false);

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
