using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int dice;
    public int damage;
    public Creep target;
    public float ttl = 1f;
    private void Update()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime / ttl);
            ttl -= Time.deltaTime;
            if (ttl < 0f)
            {
                target.Hurt(dice, damage);
                Destroy(gameObject);
            }
        }
    }
}
