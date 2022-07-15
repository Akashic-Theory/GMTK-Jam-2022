using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    int value;

    void Roll()
    {
        value = Random.Range(1, 7);
    }
}
