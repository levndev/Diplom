using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightGenerator : MonoBehaviour
{
    [SerializeField] private GameObject hoveringLightPrefab;
    [SerializeField] private GameObject wallLightPrefab;
    [SerializeField] private Comparison.Type comparison;
    [SerializeField] private float minDistance;
    [SerializeField] private int maxAmount;
    [SerializeField] private float maxWallAttachDistance;

    private class LightData
    {
        public Vector3 position;
        public Vector3 direction;
        public bool attachedToWall = false;
        public LightData(Vector3 position, Vector3 direction)
        {
            this.position = position;
            this.direction = direction;
        }
    }

    public void GenerateLights(float[][][] noise, TileData[][][] tiles, Vector3Int size, GameObject levelGeometry)
    {
        List<LightData> lights = new();

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
                            foreach(var other in lights)
                            {
                                if ((current - other.position).magnitude < minDistance)
                                {
                                    placeLight = false;
                                }
                            }
                        }
                        if (placeLight)
                        {
                            lights.Add(new LightData(current, Vector3.zero));
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

        for (int i = 0; i < lights.Count; i++)
        {
            var directions = GenerationUtils.OrthoDirections;
            List<RaycastHit> hits = new();
            foreach(var direction in directions)
            {
                if (Physics.Raycast(lights[i].position, direction, out var hit, maxWallAttachDistance))
                {
                    hits.Add(hit);
                }
            }
            if (hits.Count > 0)
            {
                var closest = hits.OrderBy(x => x.distance).First();
                lights[i].position = closest.point;
                lights[i].direction = closest.normal;
                lights[i].attachedToWall = true;
            }
        }

        foreach (var lightData in lights)
        {
            if (lightData.attachedToWall)
            {
                if (wallLightPrefab != null)
                {
                    var light = Instantiate(wallLightPrefab, levelGeometry.transform);
                    light.transform.localPosition = lightData.position;
                    light.transform.rotation =
                        Quaternion.LookRotation(lightData.direction, Vector3.up);
                }
            }
            else
            {
                if (hoveringLightPrefab!= null)
                {
                    var light = Instantiate(hoveringLightPrefab, levelGeometry.transform);
                    light.transform.localPosition = lightData.position;
                }
            }
        }

        Debug.Log(string.Format("{0} Lights generated", lights.Count));
    }
}
