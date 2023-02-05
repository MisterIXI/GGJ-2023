using UnityEngine;
using System;

public class WaterInteraction : IInteractable
{
    private int _index;
    private InteractionController _interactionController;
    public WaterInteraction(InteractionController interactionController, int index)
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

    }

    public void OnSelection(Tile tile)
    {

    }
}