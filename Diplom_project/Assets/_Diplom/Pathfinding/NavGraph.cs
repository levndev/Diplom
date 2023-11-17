using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class NavGraph : MonoBehaviour
{
    private class NavData
    {
        public bool traversible;
        public List<NavPath> cachedPaths = new();

        public NavData(bool traversible)
        {
            this.traversible = traversible;
        }

    }

    private class PointData
    {
        public bool visited;
        public float costSoFar;
        public Vector3Int cameFrom;
    }

    private NavData[][][] navData;


    private PointData[][][] points;
    private Vector3Int size;

    public void UpdateGeometry(TileData[][][] tiles, Vector3Int geometrySize)
    {
        size = geometrySize;
        int height = size.y;
        int width = size.x;
        int length = size.z;

        navData = new NavData[width][][];
        for (int x = 0; x < width; x++)
        {
            navData[x] = new NavData[height][];
            for (int y = 0; y < height; y++)
            {
                navData[x][y] = new NavData[length];
                for (int z = 0; z < size.z; z++)
                {
                    navData[x][y][z] = new NavData(tiles[x][y][z].tile == Tile.None);
                }
            }
        }

        points = new PointData[size.x][][];
        for (int x = 0; x < size.x; x++)
        {
            points[x] = new PointData[size.y][];
            for (int y = 0; y < size.y; y++)
            {
                points[x][y] = new PointData[size.z];
                for (int z = 0; z < size.z; z++)
                {
                    points[x][y][z] = new()
                    {
                        visited = false,
                        costSoFar = 0,
                    };
                }
            }
        }
    }

    public NavPath AStar(Vector3Int start, Vector3Int goal, bool cache)
    {
        float heuristic(Vector3Int a, Vector3Int b)
        {
            //return (b - a).magnitude;
            return Mathf.Abs(b.x - a.x) + Mathf.Abs(b.y - a.y) + Mathf.Abs(b.z - a.z);
        }

        if (start == goal)
        {
            NavPath result = new();
            result.points.Add(goal);
            return result;
        }

        if (navData.ByVec(start).cachedPaths.Count > 0)
        {
            foreach (var cachedPath in navData.ByVec(start).cachedPaths)
            {
                if (cachedPath.start == start && cachedPath.goal == goal)
                {
                    return cachedPath;
                }
            }
        }


        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    points[x][y][z].visited = false;
                    points[x][y][z].costSoFar = 0;
                }
            }
        }

        PriorityQueue<Vector3Int, float> frontier = new();
        frontier.Enqueue(start, 0);
        points.ByVec(start).visited = true;
        points.ByVec(start).costSoFar = 0;
        bool found = false;
        while(frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == goal)
            {
                found = true;
                break;
            }

            foreach (var direction in GenerationUtils.OrthoDirections)
            {
                var next = current + direction;
                if (GenerationUtils.InBounds(size, next)
                    && navData.ByVec(next).traversible)
                {
                    var nextPoint = points.ByVec(next);
                    var newCost = points.ByVec(current).costSoFar + 1.0f;
                    if (!nextPoint.visited
                        || newCost < nextPoint.costSoFar)
                    {
                        nextPoint.visited = true;
                        nextPoint.costSoFar = newCost;
                        nextPoint.cameFrom = current;
                        float priority = newCost + heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                    }
                }
            }
        }


        NavPath path = new();

        if (found)
        {
            bool building = true;
            PointData current = points.ByVec(goal);
            path.points.Add(goal);
            while (building)
            {
                path.points.Add(current.cameFrom);
                if (current.cameFrom == start)
                    building = false;
                current = points.ByVec(current.cameFrom);
            }
            path.goal = goal;
            path.start = start;
        }
        else
        {
            return null;
        }

        if (cache)
        {
            NavPath goalToStart = new();
            goalToStart.goal = start;
            goalToStart.start = goal;
            goalToStart.points = path.points.ToList();
            navData.ByVec(goal).cachedPaths.Add(goalToStart);
        }
        path.points.Reverse();
        if (cache)
            navData.ByVec(start).cachedPaths.Add(path);
        return path;
    }

}
