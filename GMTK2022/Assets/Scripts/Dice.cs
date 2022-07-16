using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    private int _val;
    public int val
    {
        get => _val;
        set
        {
            if (value < 1 || value > 6)
            {
                return;
            }

            _val = value;
            transform.rotation = dieAngles[_val - 1];
        }
    }

    private Quaternion[] dieAngles = new Quaternion[6]
    {
        Quaternion.LookRotation(Vector3.forward, Vector3.up),
        Quaternion.LookRotation(Vector3.down, Vector3.forward),
        Quaternion.LookRotation(Vector3.forward, Vector3.left),
        Quaternion.LookRotation(Vector3.forward, Vector3.right),
        Quaternion.LookRotation(Vector3.up, Vector3.forward),
        Quaternion.LookRotation(Vector3.forward, Vector3.down),
    };
    
    private void Awake()
    {
        Roll();
    }

    void Roll()
    {
        val = Random.Range(1, 7);
    }
}
