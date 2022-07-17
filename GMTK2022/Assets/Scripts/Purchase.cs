using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Purchase : MonoBehaviour
{
    public DiceTray tray;
    public DiceSocket[] sockets;
    public TMP_Text display;

    public Action OnBuy;

    public int cost;
    private (int, int) validRolls;

    private void Awake()
    {
        OnBuy = () => { };
        RandomizeRolls();
        UpdateDisplay();
    }

    private void Start()
    {
        foreach(DiceSocket socket in sockets)
        {
            socket.OnSocket += CheckStatus;
        }
    }

    public void CheckStatus()
    {
        if (sockets.All(s => s.held) && tray.dicePool >= cost)
        {
            foreach (DiceSocket socket in sockets)
            {
                socket.Pop();
            }

            tray.dicePool -= cost;
            cost++;

            RandomizeRolls();
            UpdateDisplay();

            OnBuy();
        }
    }

    private void CheckStatus(Dice dice)
    {
        CheckStatus();
    }

    private void RandomizeRolls()
    {
        validRolls = (Random.Range(1, 7), Random.Range(1, 7));

        while(validRolls.Item1 == validRolls.Item2)
        {
            validRolls.Item2 = Random.Range(1, 7);
        }

        foreach(DiceSocket socket in sockets)
        {
            socket.valid = new bool[] { false, false, false, false, false, false };
            socket.valid[validRolls.Item1 - 1] = true;
            socket.valid[validRolls.Item2 - 1] = true;
        }
    }

    private void UpdateDisplay()
    {
        display.text = $"{cost}\n" +
            $"{validRolls}";
    }
}
