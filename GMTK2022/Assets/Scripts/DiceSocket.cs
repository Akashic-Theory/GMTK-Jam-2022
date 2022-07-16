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
    private Dice held;

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

    public void Attach(Dice dice)
    {
        if (valid[dice.val - 1])
        {
            Pop();
            held = dice;
            var diceTransform = dice.transform;
            diceTransform.position = transform.position;
            diceTransform.parent = transform;
            OnSocket(dice);
        }
    }

    public void Pop()
    {
        if (held)
        {
            OnPop(held);
        }

        held = null;
    }
}