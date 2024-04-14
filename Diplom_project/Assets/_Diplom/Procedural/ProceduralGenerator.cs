using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class ProceduralGenerator : MonoBehaviour
{
    [SerializeField] private bool drawHollowedGizmos;
    [SerializeField] private GameObject levelGeometry;
    [SerializeField] private BlockGenerator blockGenerator;
    [SerializeField] private LightGenerator lightGenerator;
    [SerializeField] private BlobMapper blobMapper;
    [SerializeField] private SpawnAndExitGenerator spawnExitGenerator;
    [SerializeField] private EnemyGenerator enemyGenerator;
    [SerializeField] private NavGraph navGraph;

    [SerializeField] private Vector3Int generatedSize;
    [SerializeField] private PerlinSettings perlinSettings;

    [SerializeField] private Comparison.Type localMaximumComparison;

    [SerializeField] private bool drawLocalMaximumGizmos;

    private List<GameObject> spawnedGameobjects = new();

    private List<Vector3Int> localMaximums;

    private float seed;

    private TileData[][][] tiles;

    private NavPath path;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        ChangeSeed();
        Regenerate();
    }

    public void ChangeSeed()
    {
        seed = UnityEngine.Random.Range(-1000.0f, 1000.0f);
        if (blockGenerator != null)
            blockGenerator.ChangeSeed();
    }

    public void Regenerate()
    {

        var startTime = DateTime.Now;
        Clear();


        float[][][] noise;

        int height = generatedSize.y;
        int width = generatedSize.x;
        int length = generatedSize.z;

        noise = new float[width][][];
        for (int x = 0; x < width; x++)
        {
            noise[x] = new float[height][];
            for (int y = 0; y < height; y++)
            {
                noise[x][y] = new float[length];
            }
        }


        float scaleTarget = Mathf.Min(width, height, length);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    float scaledX = x / scaleTarget;
                    float scaledY = y / scaleTarget;
                    float scaledZ = z / scaleTarget;
                    noise[x][y][z] = Perlin.Noise3D(scaledX, scaledY, scaledZ, seed, perlinSettings);
                }
            }
        }


        localMaximums = new();

        void checkMaximum(BlobData blob, Vector3Int current)
        {
            bool add = true;
            foreach (var neighbour in GenerationUtils.GetNeighbours(noise, current,
                    generatedSize, GenerationUtils.AllDirections))
            {
                if (!Comparison.Compare(noise.ByVec(current),
                    neighbour, localMaximumComparison))
                {
                    add = false;
                }
            }

            if (add)
            {
                blob.localMaximums.Add(current);
                localMaximums.Add(current);
            }
        }

        spawnedGameobjects = new();

        if (blockGenerator != null)
        {
            tiles = blockGenerator.GenerateBlocks(noise, generatedSize, levelGeometry);

            if (blobMapper != null)
            {
                (var _, var biggestBlob) = blobMapper.MapBlobs(tiles, generatedSize);

                foreach(var point in biggestBlob.tiles)
                {

                    if (tiles.ByVec(point).tile == Tile.None
                        && !tiles.ByVec(point).hollowed)
                    {
                        checkMaximum(biggestBlob, point);
                    }
                }
                var spawnAndExit = spawnExitGenerator.Generate(biggestBlob, tiles, generatedSize);

                spawnedGameobjects.AddRange(spawnAndExit);
                if (lightGenerator != null)
                {
                    lightGenerator.GenerateLights(tiles,
                        biggestBlob.localMaximums,
                        levelGeometry);
                }

                if (navGraph != null)
                {
                    navGraph.UpdateGeometry(tiles, generatedSize);

                    if (enemyGenerator != null)
                    {
                        var enemies = enemyGenerator.Generate(biggestBlob.localMaximums,
                            navGraph);
                        spawnedGameobjects.AddRange(enemies);
                    }
                }
            }
        }

        Debug.Log(string.Format("{0} Time", (DateTime.Now - startTime).TotalSeconds));
    }


    public void Clear()
    {
        foreach (Transform child in levelGeometry.transform)
        {
            Destroy(child.gameObject);
        }
        if (spawnedGameobjects != null)
        {
            for (int i = spawnedGameobjects.Count - 1; i >= 0; --i)
            {
                Destroy(spawnedGameobjects[i]);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawHollowedGizmos)
        {
            Gizmos.color = UnityEngine.Color.red;
            for (int y = 0; y < generatedSize.y; y++)
            {
                for (int x = 0; x < generatedSize.x; x++)
                {
                    for (int z = 0; z < generatedSize.z; z++)
                    {
                        if (tiles[x][y][z].hollowed)
                        {
                            Gizmos.DrawWireCube(new Vector3(x, y, z), Vector3.one);
                        }
                    }
                }
            }
        }
        if (path != null)
        {
            Gizmos.color = UnityEngine.Color.red;
            foreach (var point in path.points)
            {
                Gizmos.DrawWireCube(point, Vector3.one);
            }
        }
        if (drawLocalMaximumGizmos)
        {
            Gizmos.color = UnityEngine.Color.red;
            foreach (var point in localMaximums)
            {
                Gizmos.DrawWireCube(point, Vector3.one);
            }
        }
    }
}
