using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicAgentBehaviour : MonoBehaviour
{
    [SerializeField] private NavAgent agent;

    [SerializeField] private StateMachine stateMachine;

    public void SetPatrol(List<Vector3Int> patrol)
    {
        PatrolState patrolState = new(
            agent,
            patrol,
            PatrolState.PatrolMode.PingPong,
            PatrolState.PatrolDirection.Forward);

        stateMachine.AddState(patrolState);
        stateMachine.SwitchToState(PatrolState.StateName, false);
    }

    private void Start()
    {
        stateMachine?.AddState(new LookAroundState(transform, new Vector3(0, 180, 0), 2));
    }
}
