using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField] private SpawnAndExitGenerator spawnExitGenerator;

    [SerializeField] private Vector3Int generatedSize;
    [SerializeField] private PerlinSettings perlinSettings;

    private float seed;

    private TileData[][][] tiles;



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

        if (blockGenerator != null)
        {
            tiles = blockGenerator.GenerateBlocks(noise, generatedSize, levelGeometry);

            if (spawnExitGenerator != null)
            {
                spawnExitGenerator.Generate(tiles, generatedSize);
            }

            if (lightGenerator != null)
            {
                lightGenerator.GenerateLights(noise, tiles, generatedSize, levelGeometry);
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
    }

    private void OnDrawGizmos()
    {
        if (drawHollowedGizmos)
        {
            Gizmos.color = Color.red;
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
    }
}
