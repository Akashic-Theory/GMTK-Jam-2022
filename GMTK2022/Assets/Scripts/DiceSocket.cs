using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DiceSocket : MonoBehaviour
{
    public Action<Dice> OnSocket;
    public bool[] valid = {true, true, true, true, true, true};

    private void Awake()
    {
        OnSocket = dice => { };
        gameObject.layer |= LayerMask.NameToLayer("Socket");
    }

    public void Attach(Dice dice)
    {
        if (valid[dice.val - 1])
        {
            var diceTransform = dice.transform;
            diceTransform.position = transform.position;
            diceTransform.parent = transform;
            OnSocket(dice);
        }
    }
}