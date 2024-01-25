using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NavAgent : MonoBehaviour
{
    [NonSerialized] private NavGraph navGraph;

    [SerializeReference] private IAgentMovement agentMovement;
    [SerializeField] private float pointApproachDistance;

    private NavPath currentPath;
    private int nextPathPoint;

    [SerializeField] public UnityEvent OnPathEndReached;

    [SerializeField] private bool drawPathGizmo;

    public void SetNavGraph(NavGraph navGraph)
    {
        this.navGraph = navGraph;
    }

    private void FixedUpdate()
    {
        if (currentPath != null &&
            agentMovement != null &&
            agentMovement.GetUpdateMode() == IAgentMovement.UpdateMode.FixedUpdate)
        {
            agentMovement.MoveTowards(currentPath[nextPathPoint]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPath != null &&
            agentMovement != null &&
            agentMovement.GetUpdateMode() == IAgentMovement.UpdateMode.Update)
        {
            agentMovement.MoveTowards(currentPath[nextPathPoint]);
        }
        if (currentPath != null)
        {
            var diff = (currentPath[nextPathPoint] - transform.position).magnitude;
            if (diff < pointApproachDistance)
            {
                nextPathPoint++;
                if (nextPathPoint >= currentPath.Length())
                {
                    currentPath = null;
                    nextPathPoint = 0;
                    OnPathEndReached?.Invoke();
                }
            }
        }
    }

    public void MoveTo(Vector3Int goal)
    {
        if (navGraph != null)
        {
            var path = navGraph.AStar(transform.position.ToVector3Int(), goal, true);
            if (path != null)
            {
                currentPath = path;
                nextPathPoint = 0;
            }
        }
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
