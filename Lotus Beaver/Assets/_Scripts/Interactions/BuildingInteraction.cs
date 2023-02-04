using UnityEngine;

using System;

public class BuildingInteraction : IInteractable
{
    private BuildingPreset _settings;
    public BuildingInteraction(BuildingPreset settings)
    {
        _settings = settings;
    }

    public void OnInteract(Tile tile)
    {
        // instantiate building
        GameObject building = GameObject.Instantiate(_settings.buildingPrefab, tile.transform.position, Quaternion.identity);
    }

    public void OnSelection(Tile tile)
    {

    }
}