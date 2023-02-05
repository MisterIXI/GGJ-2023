using UnityEngine;
using System;

public class EarthInteraction : IInteractable
{
    private int _index;
    private InteractionController _interactionController;
    public EarthInteraction(InteractionController interactionController, int index)
    {
        InteractionController.OnInteractionChange += OnSelectionChange;
        _index = index;
        _interactionController = interactionController;
    }

    public void OnSelectionChange(int index)
    {
        if (_index == index && _interactionController.BuildPreviewSpriteRenderer != null)
        {
            _interactionController.BuildPreviewSpriteRenderer.sprite = null;
        }
    }
    public void OnInteract(Tile tile)
    {
        if (tile.TileElement?.TileElementType != TileElementType.Root && RessourceManager.earth >= GameSettingsManager.GameSettings().EarthPlacementCost)
        {
            RessourceManager.earth -= GameSettingsManager.GameSettings().EarthPlacementCost;
            TileManager.SetTileElementType(tile, TileElementType.Earth);
            SoundManager.PlayPlanting();
        }
        else
            SoundManager.PlayError();
    }

    public void OnSelection(Tile tile)
    {

    }
}