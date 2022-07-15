using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTile = new Dictionary<TileBase, TileData>();

    private LayerMask layer;

    private void Awake()
    {
        foreach(TileData d in tileDatas)
        {
            foreach (TileBase t in d.tiles)
            {
                dataFromTile.Add(t, d);
            }
        }

        layer = 1 << LayerMask.NameToLayer("Ground");
    }

    private void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Make a ray from mouse position to the plane
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            Vector3 mousePos;

            Debug.Log(layer.value);

            // If the ray hit the plane, find and render path
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                mousePos = hit.point;

                Vector3Int cell = map.WorldToCell(mousePos);
                TileBase tile = map.GetTile(cell);

                Debug.Log("This tile at " + cell + " is a " + tile + " tile.");

                Debug.Log("this tile is " + dataFromTile[tile].placeable);
            }
        }
    }
}
