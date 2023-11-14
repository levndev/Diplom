using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        List<T> result = new();

        foreach (var vector in directions)
        {
            var next = pos + vector;
            if (InBounds(size, next))
                result.Add(data[next.x][next.y][next.z]);
        }

        return result;
    }

    public static bool InBounds(Vector3Int size, Vector3Int pos)
    {
        return pos.x >= 0 && pos.x < size.x
        && pos.y >= 0 && pos.y < size.y
        && pos.z >= 0 && pos.z < size.z;
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
                    if (x == 0 && y == 0 && z == 0)
                        continue;
                    result.Add(new(x, y, z));
                }
            }
        }
        return result;
    }
}
