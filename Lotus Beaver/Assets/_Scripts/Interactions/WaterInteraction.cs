using UnityEngine;
using System;

public class WaterInteraction : IInteractable
{
    public void OnInteract(Tile tile)
    {
        TileManager.SetTileElementType(tile, TileElementType.Water);
    }

    public void OnSelection(Tile tile)
    {

    }
}