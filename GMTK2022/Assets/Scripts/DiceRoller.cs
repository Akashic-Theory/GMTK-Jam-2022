using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DiceRoller : MonoBehaviour
{
    [Header("Roll Parameters")] 
    [SerializeField] private float rotStrength = 2f;
    [SerializeField] private float velocity = 1f;
    
    [HideInInspector]
    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 dir)
    {
        rb.angularVelocity = new Vector3(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f)).normalized * rotStrength;
        rb.velocity = dir * velocity * Random.Range(.7f, 1f);
    }
}
