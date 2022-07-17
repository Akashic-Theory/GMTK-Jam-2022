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
    protected Dice DicePrefab;
    [SerializeField]
    protected int hp = 4;
    [SerializeField] 
    protected int growth = 1;
    [SerializeField]
    protected int worth;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int[] resist = new int[6] { 1, 2, 4, 0, 4, 2 };

    [SerializeField]
    protected PlaySound soundCreator;

    protected int value;
    protected int pathIndex = 1;
    protected Vector3 target;
    private EventRedirect redirect;
    private bool isDead = false;

    public void Init(float growMod, int reward, int waveNum)
    {
        hp += Mathf.FloorToInt(growth * waveNum * growMod);
        worth = reward;
    }

    private void Awake()
    {
        value = Random.Range(1, 7);
        Dice die = Instantiate(DicePrefab, DieSocket);
        die.val = value;

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
                Game.Creeps.Remove(this);
            }
        }
    }

    public void Hurt(int dice, int damage)
    {
        soundCreator.PlayHurtSound();
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
            Game.Creeps.Remove(this);
        }
    }

    private void Die()
    {
        StartCoroutine(FallThroughGround());
        DiceTray.tray.dicePool += worth;
    }

    IEnumerator FallThroughGround()
    {
        Vector3 orig = transform.position;
        Vector3 destination = new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z);
        float time = 0;
        while (time <= 1)
        {
            transform.position = Vector3.Lerp(orig, destination, time);
            time += Time.deltaTime / 2;
            yield return null;
        }

        Destroy(gameObject);
    }
}
