using UnityEngine;

using System;
using System.Resources;

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
        if(RessourceManager.EnoughResources(_settings.earthCost, _settings.waterCost))
        {
            RessourceManager.UseResources(_settings.earthCost, _settings.waterCost);
            GameObject building = GameObject.Instantiate(_settings.buildingPrefab, tile.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Not enough Resources!");
        }
        
    }

    public void OnSelection(Tile tile)
    {

    }
}