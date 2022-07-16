using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Vector3[] points;

    private void Awake()
    {
        Game.path = new List<Vector3>(points);
    }
}
