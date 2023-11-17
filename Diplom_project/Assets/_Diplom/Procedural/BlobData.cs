using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobData
{
    public List<Vector3Int> tiles = new();
    public int size = 0;
    public Color color;
    public List<Vector3Int> localMaximums = new();
}
