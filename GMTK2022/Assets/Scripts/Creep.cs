using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Creep : MonoBehaviour
{
    [SerializeField]
    protected int hp = 4;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int[] resist = new int[6] { 1, 2, 4, 0, 4, 2 };

    protected int value;
    protected int pathIndex = 1;
    protected Vector3 target;

    private void Awake()
    {
        value = Random.Range(1, 7);
        target = Game.path[pathIndex];
    }

    private void FixedUpdate()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, Time.fixedDeltaTime * speed);

        if (gameObject.transform.position.Equals(target))
        {
            pathIndex++;

            if(pathIndex < Game.path.Count)
            {
                target = Game.path[pathIndex];
            }
            else
            {
                Game.DamagePlayer(1);
                Die();
            }
        }
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
