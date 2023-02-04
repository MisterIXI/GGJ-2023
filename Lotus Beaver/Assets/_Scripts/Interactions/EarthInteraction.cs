using UnityEngine;
using System;

public class EarthInteraction : IInteractable
{
    public void OnInteract(Tile tile)
    {
        TileManager.SetTileElementType(tile, TileElementType.Earth);

    }

    public void OnSelection(Tile tile)
    {

    }
}