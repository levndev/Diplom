using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State currentState;
    private Dictionary<string, State> allStates = new();

    public void AddState(State state)
    {
        if (state != null)
        {
            string name = state.Name();
            if (allStates.ContainsKey(name))
            {
                allStates[name]?.OnDeactivate();
                allStates.Remove(name);
            }
            allStates.Add(name, state);
            state.requestSwitchState = SwitchToState;
            state.Reset();
            if (currentState != null && currentState.Name() == name)
            {
                currentState = null;
                SwitchToState(name, false);
            }
        }
    }

    public void SwitchToState(string name, bool reset)
    {
        if (currentState != null && currentState.Name() == name)
            return;
        if (allStates.ContainsKey(name))
        {
            currentState?.OnDeactivate();
            currentState = allStates[name];
            if (reset)
                currentState?.Reset();
            currentState?.OnActivate();
        }
    }

    private void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    private void Update()
    {
        currentState?.Update();
    }
}
