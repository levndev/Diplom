using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicAgentBehaviour : MonoBehaviour
{
    public enum PatrolMode
    {
        PingPong,
        Circle
    }

    private enum State
    {
        Idle,
        PatrollingForward,
        PatrollingBackward
    }


    [SerializeField] private NavAgent agent;
    [SerializeField] private List<Vector3Int> patrol;
    [SerializeField] public PatrolMode patrolMode;

    private int currentPatrolPoint;
    private State state;

    public void SetPatrol(List<Vector3Int> patrol)
    {
        this.patrol = patrol;
        OnPatrolUpdate();
    }

    private void OnPatrolUpdate()
    {
        currentPatrolPoint = 0;
        if (patrol != null && patrol.Count > 0)
        {
            state = State.PatrollingForward;
            agent.MoveTo(patrol[currentPatrolPoint]);
        }
        else
        {
            state = State.Idle;
        }
    }

    private void Start()
    {
        if (agent != null)
        {
            //agent.OnPathEndReached.AddListener(OnAgentGoalReached);
            OnPatrolUpdate();
        }
        else
        {
            state = State.Idle;
        }
    }

    public void OnAgentGoalReached()
    {
        if (state == State.PatrollingForward)
            currentPatrolPoint++;
        else if (state == State.PatrollingBackward)
            currentPatrolPoint--;

        if (currentPatrolPoint >= patrol.Count)
        {
            switch (patrolMode)
            {
                case PatrolMode.PingPong:
                    currentPatrolPoint = patrol.Count - 2;
                    state = State.PatrollingBackward;
                    break;
                case PatrolMode.Circle:
                    currentPatrolPoint = 0;
                    state = State.PatrollingForward;
                    break;
                default:
                    break;
            }
        }
        if (currentPatrolPoint < 0)
        {
            switch (patrolMode)
            {
                case PatrolMode.PingPong:
                    currentPatrolPoint = 1;
                    state = State.PatrollingForward;
                    break;
                case PatrolMode.Circle:
                    currentPatrolPoint = patrol.Count - 1;
                    state = State.PatrollingBackward;
                    break;
                default:
                    break;
            }
        }
        agent.MoveTo(patrol[currentPatrolPoint]);
    }
}
