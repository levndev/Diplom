using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class SpawnAndExitGenerator : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private GameObject exitPrefab;
    [SerializeField] private bool drawSpawnExitGizmos;

    private Vector3Int spawnPos;
    private Vector3Int exitPos;

    public Tuple<Vector3Int, Vector3Int> Generate(BlobData blob, TileData[][][] tiles, Vector3Int size)
    {
        spawnPos = Vector3Int.zero;
        exitPos = Vector3Int.zero;

        float diagonal = (size - Vector3Int.one * 5).magnitude;

        float maxDistance = float.MinValue;
        bool found = false;
        foreach (var i in blob.tiles)
        {
            foreach (var j in blob.tiles)
            {
                if (i != j)
                {
                    float distance = (j - i).magnitude;
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        spawnPos = i;
                        exitPos = j;
                        if (distance > diagonal)
                            found = true;
                    }
                }
                if (found)
                    break;
            }
            if (found)
                break;
        }

        if (spawnPrefab != null)
        {
            var spawn = Instantiate(spawnPrefab);
            tiles[spawnPos.x][spawnPos.y][spawnPos.z].tile = Tile.None;
            blob.localMaximums.Remove(spawnPos);
            spawn.transform.position = spawnPos;
        }

        if (exitPrefab != null)
        {
            var exit = Instantiate(exitPrefab);
            tiles[exitPos.x][exitPos.y][exitPos.z].tile = Tile.None;
            blob.localMaximums.Remove(exitPos);
            exit.transform.position = exitPos;
        }


        return new Tuple<Vector3Int, Vector3Int>(spawnPos, exitPos);
    }

    private void OnDrawGizmos()
    {
        if (drawSpawnExitGizmos)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(spawnPos, Vector3.one);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(exitPos, Vector3.one);
        }
    }

}
