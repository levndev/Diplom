using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using static NavAgent;

public class NavAgent : MonoBehaviour
{
    public enum PathMode
    {
        Strict,
        Shortening
    };

    [NonSerialized] private NavGraph navGraph;

    [SerializeReference] private IAgentMovement agentMovement;
    [SerializeField] private float pointApproachDistance;

    private NavPath currentPath;
    private int nextPathPoint;
    private PathMode pathMode;

    private Nullable<Vector3> directMoveGoal = null;

    [SerializeField] public UnityEvent OnGoalReached;

    [SerializeField] private bool drawPathGizmo;


    public void setMoveSpeed(float moveSpeed)
    {
        agentMovement.setMoveSpeed(moveSpeed);
    }
    public void setTurnSpeed(float turnSpeed)
    {
        agentMovement.setTurnSpeed(turnSpeed);
    }

    public void SetNavGraph(NavGraph navGraph)
    {
        this.navGraph = navGraph;
    }

    private void FixedUpdate()
    {
        if (agentMovement != null &&
            agentMovement.GetUpdateMode() == IAgentMovement.UpdateMode.FixedUpdate)
        {
            if (currentPath != null)
            {
                agentMovement.MoveTowards(currentPath[nextPathPoint]);
            }
            else if (directMoveGoal != null)
            {
                agentMovement.MoveTowards(directMoveGoal.Value);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agentMovement != null &&
            agentMovement.GetUpdateMode() == IAgentMovement.UpdateMode.Update)
        {
            if (currentPath != null)
            {
                agentMovement.MoveTowards(currentPath[nextPathPoint]);
            }
            else if (directMoveGoal != null)
            {
                agentMovement.MoveTowards(directMoveGoal.Value);
            }
        }
        if (currentPath != null)
        {
            var diff = (currentPath[nextPathPoint] - transform.position).magnitude;
            if (diff < pointApproachDistance)
            {
                nextPathPoint++;
                if (nextPathPoint >= currentPath.Length())
                {
                    ResetPath();
                    OnGoalReached?.Invoke();
                }
                else if (pathMode == PathMode.Shortening)
                {
                    ShortenPath();
                }
            }
        }
        else if (directMoveGoal != null)
        {
            var diff = (directMoveGoal.Value - transform.position).magnitude;
            if (diff < pointApproachDistance)
            {
                ResetDirectMoveGoal();
                OnGoalReached?.Invoke();
            }
        }
    }

    public void ResetPath()
    {
        currentPath = null;
        nextPathPoint = 0;
    }

    public void ResetDirectMoveGoal()
    {
        directMoveGoal = null;
    }

    public void PathMoveTo(Vector3Int goal, PathMode mode = PathMode.Strict)
    {
        if (currentPath != null && currentPath.goal == goal)
            return;
        if (navGraph != null)
        {
            var path = navGraph.AStar(transform.position.ToVector3Int(), goal, true);
            if (path != null)
            {
                ResetDirectMoveGoal();
                pathMode = mode;
                currentPath = path;
                nextPathPoint = 0;
                if (pathMode == PathMode.Shortening)
                {
                    ShortenPath();
                }
            }
        }
    }

    public void ShortenPath()
    {
        for (int i = 0; i < currentPath.Length(); ++i)
        {
            int index = currentPath.Length() - 1 - i;
            var point = currentPath[index];
            var pos = transform.position;
                        var diff = point - pos;
            bool hit = Physics.Raycast(pos, diff.normalized, diff.magnitude, LayerMask.GetMask("LevelGeometry"));
            nextPathPoint = index;
            if (!hit) // visible
                break;
        }
    }

    public void DirectMoveTo(Vector3 goal)
    {
        ResetPath();
        directMoveGoal = goal;
    }

    public bool HasPath()
    {
        return currentPath != null;
    }

    public Vector3Int CurrentDestination()
    {
        return currentPath[nextPathPoint];
    }

    private void OnDrawGizmos()
    {
        if (drawPathGizmo && currentPath != null)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < currentPath.Length() - 1; i++)
            {
                var pt1 = currentPath[i];
                var pt2 = currentPath[i + 1];
                Gizmos.DrawLine(pt1, pt2);
            }
        }
    }

}
