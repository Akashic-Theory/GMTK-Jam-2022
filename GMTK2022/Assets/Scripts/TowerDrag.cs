using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TowerDrag : MonoBehaviour
{
    [SerializeField] private Tilemap map;

    private Tower tower;
    private Vector3 original;
    private Camera cam;

    private bool dragging;
    private void Awake()
    {
        tower = null;
        cam = Camera.main;
    }

    private void Update()
    {
        if (!dragging)
        {
            return;
        }

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            Vector3 pos = hit.point;
            pos.x = Mathf.Floor(pos.x) + .5f;
            pos.z = Mathf.Floor(pos.z) + .5f;

            GroundTile tile = (GroundTile)map.GetTile(map.WorldToCell(pos));

            if (tile && tile.placeable)
            {
                tower.transform.position = pos;
                return;
            }
        }
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Hover")))
        {
            tower.transform.position = hit.point;
        }
    }

    public void DragTower(InputAction.CallbackContext context)
    {
        string click = "/Mouse/leftButton";
        if (context.started && context.control.path == click)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Tower")))
            {
                tower = hit.transform.GetComponent<Tower>();
                original = tower.transform.position;
                dragging = true;
            }
        }
        else if (context.canceled && context.control.path == click && dragging)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Vector3 pos = hit.point;
                pos.x = Mathf.Floor(pos.x) + .5f;
                pos.z = Mathf.Floor(pos.z) + .5f;

                GroundTile tile = (GroundTile)map.GetTile(map.WorldToCell(pos));

                if (tile && tile.placeable)
                {
                    tower.transform.position = pos;
                    tower.Place();
                    tower = null;
                    dragging = false;
                    return;
                }
            }

            if (tower)
            {
                tower.transform.position = original;
            }
            tower = null;
            dragging = false;
        }
    }
}
