using System;
using UnityEngine;

public enum UnitType {Zombie, Cannon, Catapult}

[CreateAssetMenu(fileName = "New Wave Data", menuName = "ScriptableObjects/WaveData", order = 1)]
public class WaveData : ScriptableObject
{
    [Serializable]
    public class UnitSpawn
    {
        public UnitType type;
        public float delay;
        public float growMod;
        public int reward;
    }

    public int cost;
    public UnitSpawn[] units;
}