using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static float Dist2D(this Vector3 a, Vector3 b)
    {
        var x = a - b;
        x.y = 0;
        return x.magnitude;
    }

    public static List<Vector3> Distribute(this BoxCollider box, Vector3 padding)
    {
        Debug.Log($"Distributing points in box {box.size} with objects of size {padding}");
        List<Vector3> points = new List<Vector3>();
        Vector3 size = box.size;
        Vector3Int matrix = new Vector3Int(
            Mathf.FloorToInt(size.x / padding.x),
            Mathf.FloorToInt(size.y / padding.y),
            Mathf.FloorToInt(size.z / padding.z)
        );
        Vector3 excess = new Vector3(
            size.x % padding.x,
            size.y % padding.y,
            size.z % padding.z
        );
        Debug.Log($"size -{size}, excess - {excess}");
        Debug.Log($"minbound -{box.bounds.min} - {box.transform.TransformVector(box.bounds.min)}");
        for (int i = 0; i < matrix.x; i++)
        {
            Vector3 v = box.bounds.min + (excess + padding) / 2f;
            v += Vector3.right * padding.x * i;
            for (int j = 0; j < matrix.y; j++)
            {
                v += Vector3.up * padding.y;
                for (int k = 0; k < matrix.z; k++)
                {
                    v += Vector3.forward * padding.z;
                    points.Add(box.transform.TransformPoint(v));
                }
            }
            Debug.Log($"aaaa{v}");
        }
        Debug.Log($"Produced {points.Count} points");
        Debug.Log(points);
        return points;
    }
}
