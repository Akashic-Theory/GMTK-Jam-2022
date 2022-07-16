using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Reroll : MonoBehaviour
{
    public Action Roll;

    private void Awake()
    {
        Roll = () => { };
    }

    public void Cast(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Interactable")))
        {
            Roll();
        }
    }
}
