using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Creep : MonoBehaviour
{
    public int hp = 4;
    public int value;
    public int[] resist = new int[6] { 1, 2, 4, 0, 4, 2 };

    private void Awake()
    {
        value = Random.Range(1, 7);
    }

    public void Hurt(int dice, int damage)
    {
        var res = resist[Math.Abs(dice - value) % 6];
        if (res == 0)
        {
            return;
        }

        hp -= damage / res;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Game.Creeps.Remove(this);
        Destroy(gameObject);
    }
}
