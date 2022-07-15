using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform socket;
    public GameObject projectile;
    
    // Stats
    public int damage = 4;
    public float attackSpeed = 1f;
    public float range = 3.5f;
    private float attackDelay;

    //
    private float attackCD = 0f;
    private Creep target = null;
    private int _dice = 1;
    
    public int dice { get => _dice; set => _dice = value; }
    private void Awake()
    {
        attackDelay = 1f / attackSpeed;
    }

    private void FixedUpdate()
    {
        attackCD -= Time.fixedDeltaTime;
        if (!target || target.transform.position.Dist2D(transform.position) > range)
        {
            target = Game.Creeps.FindAll(creep => creep.transform.position.Dist2D(transform.position) <= range).FirstOrDefault();
        }
        if (_dice > 0 && target && attackCD < 0f)
        {
            var socketPosition = socket.position;
            var proj = Instantiate(projectile, socketPosition, Quaternion.LookRotation(target.transform.position - socketPosition, Vector3.up))
                .GetComponent<Projectile>();
            proj.damage = damage;
            proj.dice = _dice;
            proj.target = target;
            attackCD = attackDelay;
        }
    }
}
