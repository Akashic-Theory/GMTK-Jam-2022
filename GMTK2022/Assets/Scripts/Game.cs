using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[RequireComponent(typeof(Spawner))]
public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject creepPrefab;
    [SerializeField]
    private static GameObject gameOverMenu;
    [SerializeField]
    private int baseHealth;

    public static List<Creep> Creeps = null;
    public static List<Vector3> path;
    public static int playerHealth;

    public float spawnDelay = 2f;
    private float spawnTimer;
    private Spawner _spawner;
    private int cost = 1;
    
    private void Awake()
    {
        Creeps = new List<Creep>();

        playerHealth = baseHealth;

        gameOverMenu = GameObject.FindGameObjectWithTag("gameOverMenu");
        gameOverMenu.SetActive(false);

        spawnTimer = spawnDelay;
        _spawner = GetComponent<Spawner>();
    }

    private void Start()
    {
        _spawner.GetWaveOrder += CallWave;
    }

    private void CallWave()
    {
        Debug.Log($"Starting Wave {cost}");
        _spawner.SpawnWave(cost);
        cost++;
    }

    // private void FixedUpdate()
    // {
    //     spawnTimer -= Time.fixedDeltaTime;
    //     if (spawnTimer <= 0)
    //     {
    //         SpawnEnemy();
    //         spawnTimer = spawnDelay;
    //     }
    // }

    public static void DamagePlayer(int amount)
    {
        playerHealth -= amount;
        Debug.Log("Ouch!");

        if (playerHealth <= 0)
        {
            gameOverMenu.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("Game Over!");
        }
    }

    void SpawnEnemy()
    {
        var creep = Instantiate(creepPrefab, path[0], Quaternion.identity);
        Creeps.Add(creep.GetComponent<Creep>());
        //Debug.Log($"Spawned creep - Now {Creeps.Count}");
    }
}
