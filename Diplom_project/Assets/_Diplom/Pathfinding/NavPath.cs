using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavPath
{
    public List<Vector3Int> points = new();
    public Vector3Int start;
    public Vector3Int goal;
    public Vector3Int this[int i]
    {
        get { return points[i]; }
    }

    public int Length() => points.Count;

}
