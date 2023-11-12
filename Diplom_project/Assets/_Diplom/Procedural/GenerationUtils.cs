using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class GenerationUtils
{
    public static IReadOnlyList<Vector3Int> OrthoDirections = new List<Vector3Int>() {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.forward,
            Vector3Int.back,
    };

    public static IReadOnlyList<Vector3Int> AllDirections = makeAllDirections();

    public static List<T> GetNeighbours<T>(T[][][] data, Vector3Int pos, Vector3Int size, IReadOnlyList<Vector3Int> directions)
    {

        bool inBounds(Vector3Int point)
        {
            return point.x >= 0 && point.x < size.x
                && point.y >= 0 && point.y < size.y
                && point.z >= 0 && point.z < size.z;
        }

        List<T> result = new();

        foreach (var vector in directions)
        {
            var next = pos + vector;
            if (inBounds(next))
                result.Add(data[next.x][next.y][next.z]);
        }

        return result;
    }

    private static List<Vector3Int> makeAllDirections()
    {
        List<Vector3Int> result = new();
        foreach (int x in new int[] { -1, 0, 1 })
        {
            foreach (int y in new int[] { -1, 0, 1 })
            {
                foreach (int z in new int[] { -1, 0, 1 })
                {
                    if (x is 0 && y is 0 && z is 0)
                        continue;
                    result.Add(new(x, y, z));
                }
            }
        }
        return result;
    }
}
