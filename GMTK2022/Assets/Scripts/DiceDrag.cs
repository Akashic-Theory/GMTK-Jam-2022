using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceDrag : MonoBehaviour
{
    [SerializeField] private DiceTray tray;
    
    private Dice dice;
    private Vector3 original;
    private Camera cam;

    private bool dragging;
    private void Awake()
    {
        dice = null;
        cam = Camera.main;
    }

    private void Update()
    {
        if (!dragging)
        {
            return;
        }

        if (!Mouse.current.leftButton.isPressed)
        {
            dice.transform.position = original;
            dice = null;
            dragging = false;
            return;
        }


        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Socket")))
        {
            dice.transform.position = hit.transform.position;
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Hover")))
        {
            dice.transform.position = hit.point;
        } 
    }


    public void DragDice(InputAction.CallbackContext context)
    {
        string click = "/Mouse/leftButton";
        if (context.started && context.control.path == click)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Dice")))
            {
                dice = hit.transform.GetComponent<Dice>();

                if(!dice.socketed)
                {
                    original = dice.transform.position;
                    dragging = true;
                }
            }
        } else if (context.canceled && context.control.path == click && dragging)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Socket")))
            {
                DiceSocket socket = hit.transform.GetComponent<DiceSocket>();
                if(!socket.Attach(dice))
                    dice.transform.position = original;
            }
            else if(dice)
            {
                dice.transform.position = original;
            }
            dice = null;
            dragging = false;
        }
    }
}
