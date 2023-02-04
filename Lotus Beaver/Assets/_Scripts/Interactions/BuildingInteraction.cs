using UnityEngine;
using System;

public class BuildingInteraction : IInteractable
{
    private BuildingInteractionSettings _settings;
    public BuildingInteraction(BuildingInteractionSettings settings)
    {
        _settings = settings;
    }

    public void OnInteract(Tile tile)
    {

    }

    public void OnSelection(Tile tile)
    {

    }
}