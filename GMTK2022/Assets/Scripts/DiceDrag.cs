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
    private void Awake()
    {
        dice = null;
        cam = Camera.main;
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
                original = dice.transform.position;
                Debug.Log($"Got dice of {dice.value}");
            }
        } else if (context.performed && context.control.path == click)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Hover")))
            {
                dice.transform.position = hit.point;
                Debug.Log("Move");
            } else if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Socket")))
            {
                dice.transform.position = hit.transform.position;
                Debug.Log("Socket");
            }
        } else if (context.canceled && context.control.path == click)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Socket")))
            {
                DiceSocket socket = hit.transform.GetComponent<DiceSocket>();
                socket.Attach(dice);
            }
            else if(dice)
            {
                dice.transform.position = original;
            }
            dice = null;
        }
    }
}
