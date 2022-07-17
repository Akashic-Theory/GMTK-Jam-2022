using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DiceTray : MonoBehaviour
{
    public static DiceTray tray;

    [Header("Colliders")]
    [SerializeField] private DiceRoller diceRollPrefab;
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private Transform[] colliders;
    [SerializeField] private Transform[] extents;
    [SerializeField] private Transform cdRep;
    [SerializeField] private Transform[] bucketExtents;
    [SerializeField] float diceScale = 0.2f;
    [SerializeField] private Purchase towerPurchase;
    [SerializeField] private Purchase dicePurchase;
    [SerializeField] private Tower towerPrefab;
    [SerializeField] private Transform towerSpawn;

    [Space]
    [SerializeField] private int _dicePool;
    public int dicePool
    {
        get => _dicePool;
        set
        {
            _dicePool = value;
            poolText.text = $"Die: {value}";
        }
    }

    [SerializeField]
    private int maxRoll = 5;

    [SerializeField] float bucketSpawnSpeed = 3f;
    [SerializeField] float bucketSpawnCd = 0f;

    [SerializeField] private float rollCdMax = 3f;
    private float _rollCd;
    private float rollCd
    {
        get => _rollCd;
        set
        {
            _rollCd = value;
            if (value > 1f)
            {
                _rollCd = 1f;
            }

            cdRep.localScale = new Vector3(1f, 1f, _rollCd);
        }
    }

    [SerializeField] private float bucketScale = 0.05f;
    
    private List<Dice> dice;
    private List<DiceRoller> bucketDice;
    [SerializeField] private Vector3 padding;
    
    // UI stuff
    [SerializeField] private TMP_Text poolText;
    [SerializeField] private TMP_Text rollText;
    private Reroll rerollHandler;
    
    
    // Upgrades
    [SerializeField] private DiceSocket[] upgradeSockets;

    private void PrepUpgrades()
    {
        if (upgradeSockets.Length != 4)
        {
            Debug.LogError("Wrong num of upgrade slots");
        }

        for (int i = 0; i < 4; i++)
        {
            if (upgradeSockets[i].open)
            {
                continue;
            }
            upgradeSockets[i].open = true;
            /*
            upgradeSockets[i].valid = new bool[]{ false, false, false, false, false, false};
            int index = Random.Range(0, 6);
            upgradeSockets[i].valid[index] = true;
            */
        }
    }

    private void Awake()
    {
        if (!tray)
            tray = this;

        dice = new List<Dice>();
        bucketDice = new List<DiceRoller>();
        if (extents.Length != 2 || bucketExtents.Length != 2)
        {
            Debug.LogError("Wrong num of extents");
        }

        dicePool = dicePool;
        rerollHandler = GetComponentInChildren<Reroll>();
        rollCd = rollCdMax;
        bucketSpawnCd = 0f;
    }

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            upgradeSockets[i].open = false;
            upgradeSockets[i].OnPop += popped =>
            {
                Destroy(popped.gameObject);
            };
        }

        PrepUpgrades();

        rerollHandler.Roll += Roll;
        dicePurchase.OnBuy += BuyDice;
        towerPurchase.OnBuy += BuyTower;
    }

    private void Update()
    {
        rollCd += Time.deltaTime / rollCdMax;
        bucketSpawnCd -= Time.deltaTime;
        if (bucketDice.Count < _dicePool && bucketSpawnCd <= 0f)
        {
            Vector3 a = bucketExtents[0].position;
            Vector3 b = bucketExtents[1].position;
            Vector3 c = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            var vec = new Vector3(
                Mathf.Lerp(a.x, b.x, c.x),
                Mathf.Lerp(a.y, b.y, c.y),
                Mathf.Lerp(a.z, b.z, c.z)
            );
            DiceRoller roller = Instantiate(diceRollPrefab, vec, Quaternion.identity);
            roller.Init(Random.insideUnitSphere);
            roller.transform.localScale = Vector3.one * bucketScale;
            bucketDice.Add(roller);
            bucketSpawnCd = 1f / bucketSpawnSpeed;
        }

        while (bucketDice.Count > _dicePool)
        {
            Destroy(bucketDice[0].gameObject);
            bucketDice.RemoveAt(0);
        }
    }

    public void StealDice(Dice die)
    {
        dice.Remove(die);
    }

    private void BuyDice()
    {
        maxRoll++;
    }

    private void BuyTower()
    {
        Instantiate(towerPrefab, towerSpawn.position, Quaternion.identity);
    }

    public void Roll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Roll();
        }   
    }
    public void Roll()
    {
        if (rollCd < 1f)
        {
            return;
        }

        rollCd = 0f;
        dicePool += dice.Count;
        foreach (var die in dice)
        {
            if(die)
                Destroy(die.gameObject);
        }
        dice.Clear();

        int toRoll = Mathf.Min(dicePool, maxRoll);
        dicePool -= toRoll;
        List<Vector3> points = new List<Vector3>();

        #region Disgusting Calculation

        Vector3 diff = extents[1].position - extents[0].position;
        Vector3Int matrix = new Vector3Int(
            Mathf.FloorToInt(Mathf.Abs(diff.x) / padding.x),
            Mathf.FloorToInt(Mathf.Abs(diff.y) / padding.y),
            Mathf.FloorToInt(Mathf.Abs(diff.z) / padding.z)
        );
        Vector3 excess = new Vector3(
            Mathf.Abs(diff.x % padding.x),
            Mathf.Abs(diff.y % padding.y),
            Mathf.Abs(diff.z % padding.z)
        );
        Vector3 a = extents[0].position + diff.normalized * excess.magnitude;
        Vector3 b = extents[1].position - diff.normalized * excess.magnitude;

        for (int i = 0; i < matrix.x; i++)
        {
            for (int j = 0; j < matrix.y; j++)
            {
                for (int k = 0; k < matrix.z; k++)
                {
                    var vec = new Vector3(
                        Mathf.Lerp(a.x, b.x, (float)i / matrix.x),
                        Mathf.Lerp(a.y, b.y, (float)j / matrix.y),
                        Mathf.Lerp(a.z, b.z, (float)k / matrix.z)
                    );
                    points.Add(vec);
                }
            }
            
        }

        #endregion
        
        if (points.Count < toRoll)
        {
            Debug.LogError($"Not enough points({points.Count}) distributed to spawn dice({toRoll})");
            toRoll = points.Count;
        }

        List<DiceRoller> rollers = new List<DiceRoller>();

        for (int i = 0; i < toRoll; i++)
        {
            int index = Random.Range(0, points.Count);
            var obj = Instantiate(diceRollPrefab, points[index], Quaternion.identity);
            rollers.Add(obj);
            obj.Init(transform.right);
        }

        foreach (var roller in rollers)
        {
            StartCoroutine(SimDie(roller));
        }
    }

    private IEnumerator SimDie(DiceRoller roller)
    {
        while (!roller.rb.IsSleeping())
        {   
            yield return new WaitForFixedUpdate();
        }

        var rt = roller.transform;
        Dice die = Instantiate(dicePrefab, rt.position, rt.rotation);
        dice.Add(die);
        die.val = Random.Range(1, 7);
        die.transform.localScale = Vector3.one * diceScale;
        Destroy(roller.gameObject);
    }
}
