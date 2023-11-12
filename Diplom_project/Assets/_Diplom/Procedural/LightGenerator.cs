using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGenerator : MonoBehaviour
{
    [SerializeField] private GameObject lightPrefab;
    [SerializeField] private Comparison.Type comparison;
    [SerializeField] private float minDistance;
    [SerializeField] private int maxAmount;

    public void GenerateLights(float[][][] noise, TileData[][][] tiles, Vector3Int size, GameObject levelGeometry)
    {
        List<Vector3Int> lights = new();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    if (tiles[x][y][z].tile == Tile.None && !tiles[x][y][z].hollowed)
                    {
                        var current = new Vector3Int(x, y, z);
                        bool placeLight = true;
                        foreach (var neighbour in GenerationUtils.GetNeighbours(noise, current, size, GenerationUtils.AllDirections))
                        {
                            if (!Comparison.Compare(noise[x][y][z], neighbour, comparison))
                                placeLight = false;
                        }
                        if (placeLight)
                        {
                            foreach(var lightPos in lights)
                            {
                                if ((current - lightPos).magnitude < minDistance)
                                {
                                    placeLight = false;
                                }
                            }
                        }
                        if (placeLight)
                        {
                            lights.Add(current);
                        }
                    }
                }
            }
        }

        if (lights.Count > maxAmount)
        {
            int toDelete = lights.Count - maxAmount;
            for (int i = 0; i < toDelete; i++)
            {
                lights.RemoveAt(Random.Range(0, lights.Count));
            }
        }


        foreach(var lightPosition in lights)
        {
            var light = Instantiate(lightPrefab, levelGeometry.transform);
            light.transform.localPosition = lightPosition;
        }

        Debug.Log(string.Format("{0} Lights generated", lights.Count));
    }
}
