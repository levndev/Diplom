using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField] private List<TileVariant> tileVariants;
    public Comparison.Type comparison;
    public float threshold;

    public void ChangeSeed()
    {
        for (var i = 0; i < tileVariants.Count; i++)
        {
            tileVariants[i].seed = UnityEngine.Random.Range(-1000.0f, 1000.0f);
        }
    }

    public TileData[][][] GenerateBlocks(float[][][] noise, Vector3Int size, GameObject levelGeometry)
    {
        bool onEdge(int x, int y, int z)
        {
            return x == 0 || y == 0 || z == 0 ||
                   x == size.x - 1 || y == size.y - 1 || z == size.z - 1;
        }

        TileData[][][] tiles;

        tiles = new TileData[size.x][][];
        for (int x = 0; x < size.x; x++)
        {
            tiles[x] = new TileData[size.y][];
            for (int y = 0; y < size.y; y++)
            {
                tiles[x][y] = new TileData[size.z];
                for (int z = 0; z < size.z; z++)
                {
                    tiles[x][y][z] = new TileData();
                }
            }
        }

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    if (Comparison.Compare(noise[x][y][z], threshold, comparison) || onEdge(x, y, z))
                        tiles[x][y][z].tile = Tile.Block;
                }
            }
        }


        List<Vector3Int> toDelete = new();


        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    if (tiles[x][y][z].tile == Tile.Block)
                    {
                        var current = new Vector3Int(x, y, z);
                        bool delete = true;
                        foreach (var neighbour in GenerationUtils.GetNeighbours(tiles, current, size, GenerationUtils.OrthoDirections))
                        {
                            if (neighbour.tile == Tile.None)
                            {
                                delete = false;
                            }
                        }
                        if (delete)
                        {
                            toDelete.Add(current);
                            tiles[x][y][z].hollowed = true;
                        }
                    }
                }
            }
        }

        foreach (var vector in toDelete)
        {
            tiles[vector.x][vector.y][vector.z].tile = Tile.None;
        }


        float scaleTarget = Mathf.Min(size.x, size.y, size.z);

        if (tileVariants.Count > 0)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        if (tiles[x][y][z].tile == Tile.Block)
                        {
                            int variant = 3;
                            float scaledX = x / scaleTarget;
                            float scaledY = y / scaleTarget;
                            float scaledZ = z / scaleTarget;
                            for (int i = 0; i < tileVariants.Count; i++)
                            {
                                TileVariant tileVariant = tileVariants[i];
                                float variantNoise = Perlin.Noise3D(scaledX, scaledY, scaledZ, tileVariant.seed, tileVariant.perlinSettings);
                                if (Comparison.Compare(variantNoise, tileVariant.threshold, tileVariant.comparison))
                                    variant = i;
                            }
                            tiles[x][y][z].variant = variant;
                        }
                    }
                }
            }
        }

        int blocksGenerated = 0;

        MeshMaker meshMaker = new(size.x, size.y, size.z, 0.5f);
        meshMaker.begin();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    if (tiles[x][y][z].tile == Tile.Block)
                    {
                        blocksGenerated++;
                        meshMaker.makeCube(x, y, z, tileVariants[tiles[x][y][z].variant].uv, tiles);
                    }
                }
            }
        }

        Mesh mesh = meshMaker.end();

        levelGeometry.GetComponent<MeshFilter>().sharedMesh = mesh;
        levelGeometry.GetComponent<MeshCollider>().sharedMesh = mesh;

        Debug.Log(string.Format("{0} Blocks generated", blocksGenerated));

        return tiles;
    }
}
