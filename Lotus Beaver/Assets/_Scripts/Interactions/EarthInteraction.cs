using UnityEngine;
using System;

public class EarthInteraction : IInteractable
{
    public void OnInteract(Tile tile)
    {
        if (tile.TileElement?.TileElementType != TileElementType.Root)
        {
            TileManager.SetTileElementType(tile, TileElementType.Earth);
        }
    }

    public void OnSelection(Tile tile)
    {

    }
}