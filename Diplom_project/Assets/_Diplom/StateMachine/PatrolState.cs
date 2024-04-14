using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public static readonly string StateName = "Patrol";

    private float moveSpeed = 1;
    private float turnSpeed = 90;

    public enum PatrolMode
    {
        PingPong,
        Circle
    }

    public enum PatrolDirection
    {
        Forward,
        Backward
    }

    private NavAgent agent;
    private List<Vector3Int> route;
    private PatrolMode patrolMode;
    private PatrolDirection startPatrolDirection;

    private int currentPatrolPoint;
    private PatrolDirection currentPatrolDirection;

    public PatrolState(NavAgent agent,
        List<Vector3Int> route,
        PatrolMode mode,
        PatrolDirection direction)
        : base(StateName)
    {
        this.agent = agent;
        this.route = route;
        this.patrolMode = mode;
        this.startPatrolDirection = direction;
    }

    public override void Reset()
    {
        base.Reset();
        currentPatrolDirection = startPatrolDirection;
        currentPatrolPoint = 0;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnActivate()
    {
        base.OnActivate();
        agent.OnGoalReached.AddListener(OnAgentGoalReached);
        agent.setMoveSpeed(moveSpeed);
        agent.setTurnSpeed(turnSpeed);
        if (route != null && route.Count > 0)
        {
            agent.PathMoveTo(route[currentPatrolPoint]);
        }
    }

    public override void OnDeactivate()
    {
        agent.OnGoalReached.RemoveListener(OnAgentGoalReached);
        base.OnDeactivate();
    }

    public void OnAgentGoalReached()
    {
        if (!active)
            return;
        if (currentPatrolDirection == PatrolDirection.Forward)
            currentPatrolPoint++;
        else if (currentPatrolDirection == PatrolDirection.Backward)
            currentPatrolPoint--;

        if (currentPatrolPoint >= route.Count)
        {
            switch (patrolMode)
            {
                case PatrolMode.PingPong:
                    currentPatrolPoint = route.Count - 2;
                    currentPatrolDirection = PatrolDirection.Backward;
                    break;
                case PatrolMode.Circle:
                    currentPatrolPoint = 0;
                    currentPatrolDirection = PatrolDirection.Forward;
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
                    currentPatrolDirection = PatrolDirection.Forward;
                    break;
                case PatrolMode.Circle:
                    currentPatrolPoint = route.Count - 1;
                    currentPatrolDirection = PatrolDirection.Backward;
                    break;
                default:
                    break;
            }
        }
        requestSwitchState?.Invoke(LookAroundState.StateName, true);
    }
}
