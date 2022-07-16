using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Creep : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected Transform DieSocket;
    [SerializeField]
    protected GameObject DicePrefab;
    [SerializeField]
    protected int hp = 4;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int[] resist = new int[6] { 1, 2, 4, 0, 4, 2 };

    private Quaternion[] dieAngles = new Quaternion[6]
    {
        Quaternion.LookRotation(Vector3.forward, Vector3.up),
        Quaternion.LookRotation(Vector3.down, Vector3.forward),
        Quaternion.LookRotation(Vector3.forward, Vector3.left),
        Quaternion.LookRotation(Vector3.forward, Vector3.right),
        Quaternion.LookRotation(Vector3.up, Vector3.forward),
        Quaternion.LookRotation(Vector3.forward, Vector3.down),
    };

    protected int value;
    protected int pathIndex = 1;
    protected Vector3 target;
    private EventRedirect redirect;
    private bool isDead = false;


    private void Awake()
    {
        value = Random.Range(1, 7);
        SkullInit();
        target = Game.path[pathIndex];
        transform.rotation = Quaternion.LookRotation(target - transform.position, Vector3.up);

        redirect = gameObject.GetComponentInChildren<EventRedirect>();
    }

    private void Start()
    {
        redirect.Add("die", Die);
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target, Time.fixedDeltaTime * speed);

        if (transform.position.Equals(target))
        {
            pathIndex++;

            if(pathIndex < Game.path.Count)
            {
                target = Game.path[pathIndex];
                transform.rotation = Quaternion.LookRotation(target - transform.position, Vector3.up);
            }
            else
            {
                Game.DamagePlayer(1);
                isDead = true;
                animator.SetBool("Attack", true);
            }
        }
    }

    private void SkullInit()
    {
        GameObject die = Instantiate(DicePrefab, DieSocket);
        Debug.Log(value);
        die.transform.rotation = dieAngles[value - 1];
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
            isDead = true;
            animator.SetBool("Die", true);
        }
    }

    private void Die()
    {
        Game.Creeps.Remove(this);
        Destroy(gameObject);
    }
}
