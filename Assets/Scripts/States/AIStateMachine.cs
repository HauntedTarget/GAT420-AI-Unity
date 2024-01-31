using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    private Dictionary<string, AIState> states = new();

    public AIState CurrentState { get; private set; } = null;

    // Update is called once per frame
    public void Update()
    {
        CurrentState?.OnUpdate();
    }

    public void AddState(string name, AIState state)
    {
        Debug.Assert(!states.ContainsKey(name), "State machine already contains state " + name);

        states[name] = state;
    }

    public void SetState(string name)
    {
        Debug.Assert(states.ContainsKey(name), "State machine does not contains state " + name);

        var newState = states[name];

        //Don't re-enter state
        if (CurrentState == newState) return;

        //Exit current state
        CurrentState?.OnExit();
        //Set new state
        CurrentState = newState;
        //Enter new state
        CurrentState?.OnEnter();
    }
}
