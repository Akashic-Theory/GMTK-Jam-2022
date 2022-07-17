using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DiceSocket : MonoBehaviour
{
    public Action<Dice> OnSocket;
    public Action<Dice> OnPop;
    public bool[] valid = {true, true, true, true, true, true};
    private bool _open = true;
    private Collider col;
    public Dice held { get; private set; }

    public bool open
    {
        get => _open;
        set
        {
            _open = value;
            col.enabled = value;
        }
    }

    private void Awake()
    {
        col = GetComponent<Collider>();
        OnSocket = dice => { };
        OnPop = dice => { };
        held = null;
        gameObject.layer |= LayerMask.NameToLayer("Socket");
    }

    public bool Attach(Dice dice)
    {
        if (valid[dice.val - 1])
        {
            Pop();
            held = dice;

            Transform diceTransform = dice.transform;

            diceTransform.position = Vector3.zero;
            diceTransform.localScale = Vector3.one;
            diceTransform.SetParent(transform, false);

            OnSocket(dice);
            return true;
        }

        return false;
    }

    public void Pop()
    {
        if (held)
        {
            Debug.Log("Pop!");
            OnPop(held);
        }

        held = null;
    }
}