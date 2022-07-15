using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public  static float Dist2D(this Vector3 a, Vector3 b)
    {
        var x = a - b;
        x.y = 0;
        return x.magnitude;
    }
}
