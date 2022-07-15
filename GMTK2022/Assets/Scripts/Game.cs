using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject creepPrefab;

    public static List<Creep> Creeps = null;

    public float spawnDelay = 2f;
    private float spawnTimer;
    
    private void Awake()
    {
        if (Creeps == null)
        {
            Creeps = new List<Creep>();
        }

        spawnTimer = spawnDelay;
    }

    private void FixedUpdate()
    {
        spawnTimer -= Time.fixedDeltaTime;
        if (spawnTimer <= 0)
        {
            //SpawnEnemy();
            spawnTimer = spawnDelay;
        }
    }

    void SpawnEnemy()
    {
        var creep = Instantiate(creepPrefab, new Vector3(Random.Range(-2f, 3f), 0f, Random.Range(-2f, 3f)), Quaternion.identity);
        Creeps.Add(creep.GetComponent<Creep>());
        //Debug.Log($"Spawned creep - Now {Creeps.Count}");
    }
}
