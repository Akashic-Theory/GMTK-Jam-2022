using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DiceTray : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField] private DiceRoller diceRollPrefab;
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private Transform[] colliders;
    [SerializeField] private Transform[] extents;
    
    [Space]
    [SerializeField]
    private int dicePool;
    [SerializeField]
    private int maxRoll = 20;

    [SerializeField] private int simulationSteps = 100;

    private List<Dice> dice;
    [SerializeField] private Vector3 padding;

    // We're in the simulation
    private Scene _sim;
    private PhysicsScene _phys;

    private void Awake()
    {
        dice = new List<Dice>();
        if (extents.Length != 2)
        {
            Debug.LogError("Wrong num of extents");
        }
        // CreatePhysicsScene();
    }

    private void Update()
    {
        
    }

    private void CreatePhysicsScene()
    {
        _sim = SceneManager.CreateScene("Roller", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _phys = _sim.GetPhysicsScene();

        foreach (Transform col in colliders)
        {
            var ghost = Instantiate(col.gameObject, col.position, col.rotation);
            ghost.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghost, _sim);
        }
    }

    public void Roll(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        
        dicePool += dice.Count;
        foreach (var die in dice)
        {
            Destroy(die.gameObject);
        }
        dice.Clear();

        int toRoll = Mathf.Min(dicePool, maxRoll);
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
        dice.Add(Instantiate(dicePrefab, rt.position, rt.rotation));
        dicePool--;
        Destroy(roller.gameObject);
    }
}
