using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Square
{
    Vector2 position;

    Vector2 topRight;
    Vector2 bottomRight;
    Vector2 bottomLeft;
    Vector2 topLeft;

    Vector2 topCenter;
    Vector2 rightCenter;
    Vector2 bottomCenter;
    Vector2 leftCenter;

    List<Vector3> vertices;
    List<int> triangles;


    public Square(Vector2 position, float gridScale)
    {
        this.position = position;

        topRight = position + gridScale * Vector2.one / 2;
        bottomRight = topRight + gridScale * Vector2.down;
        bottomLeft = bottomRight + gridScale * Vector2.left;
        topLeft = bottomLeft + gridScale * Vector2.up;

        topCenter = topRight + gridScale / 2 * Vector2.left;
        rightCenter = bottomRight + gridScale / 2 * Vector2.up;
        bottomCenter = bottomLeft + gridScale / 2 * Vector2.right;
        leftCenter = topLeft + gridScale / 2 * Vector2.down;

        vertices = new List<Vector3>();
        triangles = new List<int>();

    }

    private int GetConfiguration(float isoValue, float[] values)
    {
        int configuration = 0;

        if (values[0] > isoValue)
        {
            configuration += 1;
        }
        if (values[1] > isoValue)
        {
            configuration += 2;
        }
        if (values[2] > isoValue)
        {
            configuration += 4;
        }
        if (values[3] > isoValue)
        {
            configuration += 8;
        }

        return configuration;
    }

    private void Interpolate(float isoValue, float[] values)
    {
        float topLerp = Mathf.InverseLerp(values[3], values[0], isoValue);
        topLerp = Mathf.Clamp01(topLerp);
        topCenter = topLeft + (topRight - topLeft) * topLerp;

        float rightLerp = Mathf.InverseLerp(values[0], values[1], isoValue);
        rightLerp = Mathf.Clamp01(rightLerp);
        rightCenter = topRight + (bottomRight - topRight) * rightLerp;

        float bottomLerp = Mathf.InverseLerp(values[2], values[1], isoValue);
        bottomLerp = Mathf.Clamp01(bottomLerp);
        bottomCenter = bottomLeft + (bottomRight - bottomLeft) * bottomLerp;

        float leftLerp = Mathf.InverseLerp(values[3], values[2], isoValue);
        leftLerp = Mathf.Clamp01(leftLerp);
        leftCenter = topLeft + (bottomLeft - topLeft) * leftLerp;
    }

    public void Triangulate(float isoValue, float[] values)
    {
        vertices.Clear();
        triangles.Clear();

        int configuration = GetConfiguration(isoValue, values);

        Interpolate(isoValue, values);

        Triangulate(configuration);
    }
    private void Triangulate(int configuration)
    {
        switch (configuration)
        {
            case 0:
                break;
            case 1:
                vertices.AddRange(new Vector3[] { topCenter, rightCenter, topCenter });
                triangles.AddRange(new int[] { 0, 1, 2 });
                break;
            case 2:
                vertices.AddRange(new Vector3[] { rightCenter, bottomRight, bottomCenter });
                triangles.AddRange(new int[] { 0, 1, 2 });
                break;
            case 3:
                vertices.AddRange(new Vector3[] { topRight, bottomRight, bottomCenter, topCenter });
                triangles.AddRange(new int[] { 0, 1, 2, 0, 2, 3 });
                break;
            case 4:
                vertices.AddRange(new Vector3[] { bottomCenter, bottomLeft, leftCenter });
                triangles.AddRange(new int[] { 0, 1, 2 });
                break;
            case 5:
                vertices.AddRange(new Vector3[] { topRight, rightCenter, bottomCenter, bottomLeft, leftCenter, topCenter });
                triangles.AddRange(new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 });
                break;
            case 6:
                vertices.AddRange(new Vector3[] { bottomRight, bottomLeft, leftCenter, rightCenter });
                triangles.AddRange(new int[] { 0, 1, 2, 0, 2, 3 });
                break;
            case 7:
                vertices.AddRange(new Vector3[] { topRight, bottomRight, bottomLeft, leftCenter, topCenter });
                triangles.AddRange(new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 });
                break;
            case 8:
                vertices.AddRange(new Vector3[] { leftCenter, topLeft, topCenter });
                triangles.AddRange(new int[] { 0, 1, 2 });
                break;
            case 9:
                vertices.AddRange(new Vector3[] { topRight, rightCenter, leftCenter, topLeft });
                triangles.AddRange(new int[] { 0, 1, 2, 0, 2, 3 });
                break;
            case 10:
                vertices.AddRange(new Vector3[] { rightCenter, bottomRight, bottomCenter, leftCenter, topLeft, topCenter });
                triangles.AddRange(new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 });
                break;
            case 11:
                vertices.AddRange(new Vector3[] { topRight, bottomRight, bottomCenter, leftCenter, topLeft });
                triangles.AddRange(new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 });
                break;
            case 12:
                vertices.AddRange(new Vector3[] { bottomCenter, bottomLeft, topLeft, topCenter });
                triangles.AddRange(new int[] { 0, 1, 2, 0, 2, 3 });
                break;
            case 13:
                vertices.AddRange(new Vector3[] { topRight, rightCenter, bottomCenter, bottomLeft, topLeft });
                triangles.AddRange(new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 });
                break;
            case 14:
                vertices.AddRange(new Vector3[] { rightCenter, bottomRight, bottomLeft, topCenter });
                triangles.AddRange(new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 });
                break;
            case 15:
                vertices.AddRange(new Vector3[] { topRight, bottomRight, bottomLeft, topLeft });
                triangles.AddRange(new int[] { 0, 1, 2, 0, 2, 3 });
                break;

        }
    }

    public Vector3[] GetVertices()
    {
        return vertices.ToArray();
    }

    public int[] GetTriangles()
    {
        return triangles.ToArray();
    }
}
