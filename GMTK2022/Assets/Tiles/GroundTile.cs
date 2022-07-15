using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class GroundTile : Tile
{
    public bool placeable;
    public bool highlighted;
    public Color highlightColor;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref UnityEngine.Tilemaps.TileData tileData)
    {
        tileData.color = highlighted ? highlightColor : Color.white;

        base.GetTileData(position, tilemap, ref tileData);
    }
}
