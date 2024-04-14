using ProceduralToolkit;
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
        public Vector3Int position;
        public Vector3 adjustedPosition;
        public Vector3 direction;
        public bool attachedToWall = false;
        public int localMaximumIndex;
        public LightData(Vector3Int position, Vector3 direction, int localMaximumIndex)
        {
            this.position = position;
            this.direction = direction;
            this.localMaximumIndex = localMaximumIndex;
        }
    }

    public void GenerateLights(TileData[][][] tiles, List<Vector3Int> localMaximums, GameObject levelGeometry)
    {
        List<LightData> lights = new();

        for (int i = 0; i < localMaximums.Count; i++)
        {
            var current = localMaximums[i];
            bool placeLight = true;
            foreach (var other in lights)
            {
                if ((current - other.position).magnitude < minDistance)
                {
                    placeLight = false;
                }
            }

            if (placeLight)
            {
                lights.Add(new LightData(current, Vector3.zero, i));
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
                //tiles.ByVec(closest.point.ToVector3Int()).tile = Tile.Light;
                localMaximums.Remove(lights[i].position);
                lights[i].adjustedPosition = closest.point;
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
                    light.transform.localPosition = lightData.adjustedPosition;
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
