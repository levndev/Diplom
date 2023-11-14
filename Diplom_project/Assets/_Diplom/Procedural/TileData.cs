using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tile
{
    None,
    Block
}

public class TileData
{
    public Tile tile = Tile.None;
    public int variant;
    public bool hollowed = false;

}

[Serializable]
public class TileVariant
{
    public Rect uv;

    public PerlinSettings perlinSettings;

    public float threshold;
    public Comparison.Type comparison;

    [NonSerialized] public float seed;
}