using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;

    //[SerializeField]
    //private List<TileData> tileDatas;

    [SerializeField]
    private Color highlightColor;

    //private Dictionary<Tile, TileData> dataFromTile = new Dictionary<Tile, TileData>();

    //TEMP
    [SerializeField]
    private GameObject tower;

    private LayerMask layer;

    private void Awake()
    {
        /*
        foreach(TileData d in tileDatas)
        {
            foreach (Tile t in d.tiles)
            {
                dataFromTile.Add(t, d);
            }
        }
        */

        layer = 1 << LayerMask.NameToLayer("Ground");
    }

    private void Start()
    {
        StartCoroutine(PlaceTower(tower));
    }

    public IEnumerator PlaceTower(GameObject tower)
    {
        Vector3Int cell = new Vector3Int();
        GroundTile tile = null;

        while (!Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (tile)
                tile.highlighted = false;

            // Make a ray from mouse position to the plane
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            Vector3 mousePos;

            // If the ray hit the plane, find and render path
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                mousePos = hit.point;

                cell = map.WorldToCell(mousePos);
                tile = (GroundTile)map.GetTile(cell);

                if (tile.placeable)
                {
                    tile.highlighted = true;
                }
            }

            yield return null;
        }

        if(!tile)
        {
            yield break;
        }

        if (tile.placeable)
        {
            Instantiate(tower, map.GetCellCenterWorld(cell), Quaternion.identity);
        }
    }
}
