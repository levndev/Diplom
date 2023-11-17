using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using System;

public class BlobMapper : MonoBehaviour
{
    private class FloodFillData
    {
        public bool visited = false;
        public Vector3Int cameFrom;
    }

    [SerializeField] private bool drawBlobGizmos;

    private List<BlobData> blobs;

    public Tuple<List<BlobData>, BlobData> MapBlobs(TileData[][][] tiles, Vector3Int size)
    {
        FloodFillData[][][] fillData;

        fillData = new FloodFillData[size.x][][];
        for (int x = 0; x < size.x; x++)
        {
            fillData[x] = new FloodFillData[size.y][];
            for (int y = 0; y < size.y; y++)
            {
                fillData[x][y] = new FloodFillData[size.z];
                for (int z = 0; z < size.z; z++)
                {
                    fillData[x][y][z] = new FloodFillData();
                }
            }
        }

        blobs = new();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    TileData tile = tiles[x][y][z];
                    if (tile.tile == Tile.None && !fillData[x][y][z].visited && !tiles[x][y][z].hollowed)
                    {
                        blobs.Add(FloodFill(tiles, fillData, new Vector3Int(x, y, z), size));
                    }
                }
            }
        }

        BlobData biggestBlob = null;
        int maxSize = int.MinValue;
        for (int i = 0; i < blobs.Count; i++)
        {
            if (blobs[i].size > maxSize)
            {
                maxSize = blobs[i].size;
                biggestBlob = blobs[i];
            }
        }

        Debug.Log(string.Format("{0} Blobs", blobs.Count));
        Debug.Log(string.Format("Biggest blob size: {0}", biggestBlob.size));

        return new(blobs, biggestBlob);
    }


    private BlobData FloodFill(TileData[][][] tiles, FloodFillData[][][] fillData,
                                Vector3Int pos, Vector3Int size)
    {
        BlobData blob = new();
        blob.color = UnityEngine.Random.ColorHSV();
        Queue<Vector3Int> frontier = new();
        frontier.Enqueue(pos);
        fillData[pos.x][pos.y][pos.z].visited = true;
        int cycles = 0;
        while (frontier.Count > 0)
        {
            Vector3Int current = frontier.Dequeue();
            blob.tiles.Add(current);
            blob.size++;
            foreach (var direction in GenerationUtils.OrthoDirections)
            {
                var next = current + direction;
                if (GenerationUtils.InBounds(size, next) && !fillData[next.x][next.y][next.z].visited)
                {
                    TileData tile = tiles[next.x][next.y][next.z];
                    if (tile.tile == Tile.None && !tile.hollowed)
                    {
                        frontier.Enqueue(next);
                        fillData[next.x][next.y][next.z].visited = true;
                        fillData[next.x][next.y][next.z].cameFrom = current;
                    }
                }
            }
            cycles++;
            if (cycles > 10000)
                break;
        }
        return blob;
    }

    private void OnDrawGizmos()
    {
        if (drawBlobGizmos && blobs != null && blobs.Count > 0)
        {
            foreach (var blob in blobs)
            {
                Gizmos.color = blob.color;
                foreach (var tile in blob.tiles)
                {
                    Gizmos.DrawWireCube(tile, Vector3.one);
                }
            }
        }
    }
}
