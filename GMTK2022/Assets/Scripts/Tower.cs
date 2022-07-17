using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private static int[] lookup;
    public DiceSocket socket;
    public Transform barrel;
    public Transform rotateAxis;
    public GameObject projectile;
    
    // Stats
    [Header("Statistics")]
    public int damage = 4;
    public float attackSpeed = 1f;
    public float range = 3.5f;
    private float attackDelay;

    // Visual Parameters
    [Header("Visual Parameters")]
    public float bobAmplitude = 1f;
    public float bobFrequency = 1f;
    public float bobPhase = 1f;
    public float rotFrequency = 1f;
    public float rotPhase = 1f;
    
    //
    private float attackCD = 0f;
    private Creep target = null;
    private int _dice = 0;
    public bool placed = false;
    
    public int dice { get => _dice; set => _dice = value; }
    private void Awake()
    {
        lookup = new int[] { 1, 2, 3, 4, 3, 2 };
        attackDelay = 1f / attackSpeed;
        gameObject.layer |= LayerMask.NameToLayer("Tower");
    }

    private void Start()
    {
        socket.OnSocket += OnSocket;
        socket.OnPop += OnPop;

        socket.OnPop += popped =>
        {
            Destroy(popped.gameObject);
        };

        socket.open = true;
    }

    private void FixedUpdate()
    {
        if (!placed)
            return;
        if (!Game.Creeps.Contains(target))
        {
            target = null;
        }
        attackCD -= Time.fixedDeltaTime;
        target = Game.Creeps.FindAll(creep => creep.transform.position.Dist2D(transform.position) <= range)
                .OrderBy(creep => lookup[Math.Abs(dice - creep.value) % 6]).FirstOrDefault();

            if (_dice > 0 && target && attackCD < 0f)
        {
            Vector3 barrelPostion = barrel.position;

            var proj = Instantiate(projectile, barrelPostion, Quaternion.LookRotation(target.transform.position - barrelPostion, Vector3.up))
                .GetComponent<Projectile>();
            proj.damage = damage;
            proj.dice = _dice;
            proj.target = target;
            attackCD = attackDelay;
        }
    }

    private void Update()
    {
        if (!placed)
            return;

        if(target)
            rotateAxis.rotation = Quaternion.LookRotation(target.transform.position - barrel.position, Vector3.up);

        Transform socketTransform = socket.transform;
        Vector3 pos = socketTransform.localPosition;
        pos.y = bobAmplitude * Mathf.Sin(bobFrequency * Time.time + bobPhase);
        socketTransform.localPosition = pos;
        socketTransform.rotation = Quaternion.LookRotation(
                            new Vector3(
                                Mathf.Sin(rotFrequency * Time.time + rotPhase),
                                0,
                                Mathf.Cos(rotFrequency * Time.time + rotPhase)),
                            Vector3.up
            );
    }

    public void Place()
    {
        placed = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void OnSocket(Dice dice)
    {
        DiceTray.tray.StealDice(dice);
        _dice = dice.val;
    }

    private void OnPop(Dice dice)
    {
        _dice = 0;
    }
}
