using ProceduralToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class MeshMaker
{
    private List<Vector3> vertices;
    private List<Vector2> uv;
    private List<Vector3> normals;
    private List<int> triangles;
    private int triangleCounter;
    private float width;
    private float height;
    private float length;
    private float halfSize;

    private long faceCount;
    private long vertexCount;
    public MeshMaker(float width, float height, float length, float tileHalfSize)
    {
        this.width = width;
        this.height = height;
        this.length = length;
        this.halfSize = tileHalfSize;
    }

    public void begin()
    {
        vertices = new();
        uv = new();
        normals = new();
        triangles = new();
        vertexCount = 0;
        faceCount = 0;
    }

    public Mesh end()
    {
        Mesh result = new Mesh();
        result.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        result.vertices = vertices.ToArray();
        result.uv = uv.ToArray();
        result.triangles = triangles.ToArray();
        result.normals = normals.ToArray();
        Debug.Log(string.Format("{0} Faces generated", faceCount));
        Debug.Log(string.Format("{0} Vertices generated", vertexCount));
        return result;
    }

    public void makeCube(int x, int y, int z, Rect uvRect, TileData[][][] tiles)
    {
        Vector3 cubeCenter = new Vector3(x, y, z);
        Vector3 topForwardLeft = cubeCenter + new Vector3(-halfSize, halfSize, halfSize);
        Vector3 topForwardRight = cubeCenter + new Vector3(halfSize, halfSize, halfSize);
        Vector3 topBackwardLeft = cubeCenter + new Vector3(-halfSize, halfSize, -halfSize);
        Vector3 topBackwardRight = cubeCenter + new Vector3(halfSize, halfSize, -halfSize);
        Vector3 bottomForwardLeft = cubeCenter + new Vector3(-halfSize, -halfSize, halfSize);
        Vector3 bottomForwardRight = cubeCenter + new Vector3(halfSize, -halfSize, halfSize);
        Vector3 bottomBackwardLeft = cubeCenter + new Vector3(-halfSize, -halfSize, -halfSize);
        Vector3 bottomBackwardRight = cubeCenter + new Vector3(halfSize, -halfSize, -halfSize);

        // Left
        Vector3Int left = new Vector3Int(x, y, z) + Vector3Int.left;
        if (!inBoundsVector3Int(left) || tiles[left.x][left.y][left.z].tile != Tile.Block)
            makeCubeFace(topForwardLeft, topBackwardLeft, bottomBackwardLeft, bottomForwardLeft, Vector3.left, uvRect);
        // Right
        Vector3Int right = new Vector3Int(x, y, z) + Vector3Int.right;
        if (!inBoundsVector3Int(right) || tiles[right.x][right.y][right.z].tile != Tile.Block)
            makeCubeFace(topBackwardRight, topForwardRight, bottomForwardRight, bottomBackwardRight, Vector3.right, uvRect);
        // Front
        Vector3Int front = new Vector3Int(x, y, z) + Vector3Int.forward;
        if (!inBoundsVector3Int(front) || tiles[front.x][front.y][front.z].tile != Tile.Block)
            makeCubeFace(topForwardLeft, bottomForwardLeft, bottomForwardRight, topForwardRight, Vector3.forward, uvRect);
        // Back
        Vector3Int back = new Vector3Int(x, y, z) + Vector3Int.back;
        if (!inBoundsVector3Int(back) || tiles[back.x][back.y][back.z].tile != Tile.Block)
            makeCubeFace(topBackwardLeft, topBackwardRight, bottomBackwardRight, bottomBackwardLeft, Vector3.back, uvRect);
        // Top
        Vector3Int top = new Vector3Int(x, y, z) + Vector3Int.up;
        if (!inBoundsVector3Int(top) || tiles[top.x][top.y][top.z].tile != Tile.Block)
            makeCubeFace(topForwardLeft, topForwardRight, topBackwardRight, topBackwardLeft, Vector3.up, uvRect);
        // Bottom
        Vector3Int bottom = new Vector3Int(x, y, z) + Vector3Int.down;
        if (!inBoundsVector3Int(bottom) || tiles[bottom.x][bottom.y][bottom.z].tile != Tile.Block)
            makeCubeFace(bottomForwardLeft, bottomBackwardLeft, bottomBackwardRight, bottomForwardRight, Vector3.down, uvRect);
    }

    private void makeCubeFace(Vector3 topLeft, Vector3 topRight, Vector3 bottomRight, Vector3 bottomLeft, Vector3 normal, Rect uvRect)
    {
        int topLeftIndex = triangleCounter++;
        vertices.Add(topLeft);
        uv.Add(new Vector2(uvRect.xMin, uvRect.yMax));
        normals.Add(normal);

        int topRightIndex = triangleCounter++;
        vertices.Add(topRight);
        uv.Add(new Vector2(uvRect.xMax, uvRect.yMax));
        normals.Add(normal);

        int bottomRightIndex = triangleCounter++;
        vertices.Add(bottomRight);
        uv.Add(new Vector2(uvRect.xMax, uvRect.yMin));
        normals.Add(normal);

        int bottomLeftIndex = triangleCounter++;
        vertices.Add(bottomLeft);
        uv.Add(new Vector2(uvRect.xMin, uvRect.yMin));
        normals.Add(normal);

        triangles.Add(topLeftIndex);
        triangles.Add(topRightIndex);
        triangles.Add(bottomLeftIndex);

        triangles.Add(bottomLeftIndex);
        triangles.Add(topRightIndex);
        triangles.Add(bottomRightIndex);

        vertexCount += 4;
        faceCount++;
    }
    private bool inBoundsVector3Int(Vector3Int p)
    {
        return p.x >= 0 && p.x < width
               && p.y >= 0 && p.y < height
               && p.z >= 0 && p.z < length;
    }

}
