using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T ByVec<T>(this T[][][] arr, Vector3Int index)
    {
        return arr[index.x][index.y][index.z];
    }
}
