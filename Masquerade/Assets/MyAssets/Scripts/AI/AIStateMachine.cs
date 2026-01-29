using UnityEngine;

public class AIStateMachine
{
    public AIState[] states;
    public AIAgent agent;
    public AiStateId currentState;

    public AIStateMachine(AIAgent agent)
    {
        this.agent = agent;
        int _numStates = System.Enum.GetNames(typeof(AiStateId)).Length;
        states = new AIState[_numStates];
    }
    public void RegisterState(AIState state)
    {
        int _index = (int)state.GetId();
        states[_index] = state;
    }

    public AIState GetState(AiStateId stateId)
    {
        int _index = (int)stateId;
        return states[_index];
    }

    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeState(AiStateId newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}
