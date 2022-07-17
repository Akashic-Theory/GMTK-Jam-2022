using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public Action GetWaveOrder;
    [SerializeField] private Creep[] creeps;
    [SerializeField] private WaveData[] waves;
    
    private Queue<WaveData.UnitSpawn> queue;
    private int waveNum = 0;
    [SerializeField] private float spawnDelay = 10f;
    private int remaining = 0;
    private void Awake()
    {
        waves = waves.OrderBy(wave => wave.cost).ToArray();
        queue = new Queue<WaveData.UnitSpawn>();
        GetWaveOrder = () => { };
    }
    private void FixedUpdate()
    {
        spawnDelay -= Time.fixedDeltaTime;
        if (queue.Count == 0)
        {
            if (Game.Creeps.Count == 0)
            {
                GetWaveOrder();
                spawnDelay = 0f;
                remaining = queue.Count;
                waveNum++;
            }
            else
            {
                return;
            }
        }

        while (spawnDelay <= 0f)
        {
            WaveData.UnitSpawn unit = queue.Dequeue();
            Creep creep = Instantiate(creeps[(int)unit.type], Game.path[0], Quaternion.identity);
            creep.Init(unit.growMod, unit.reward, waveNum);
            Game.Creeps.Add(creep);
            spawnDelay += unit.delay;
        }
    }

    public int SpawnWave(int cost)
    {
        int max = waves.Length;

        while (cost > 0)
        {
            int index = Random.Range(0, max);
            WaveData wave = waves[index];
            if (wave.cost > cost)
            {
                max = index;
                if (index == 0)
                {
                    return cost;
                }
            }
            foreach (WaveData.UnitSpawn unit in wave.units)
            {
                queue.Enqueue(unit);
            }
            cost -= wave.cost;
        }
        return cost;
    }
}
