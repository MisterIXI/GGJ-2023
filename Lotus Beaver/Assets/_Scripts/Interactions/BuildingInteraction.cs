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
        if (tile.TileElement.TileElementType == TileElementType.Earth) {
            if(RessourceManager.EnoughResources(_settings.earthCost, _settings.waterCost))
            {
                if (tile.InteractableBase == null)
                {
                    RessourceManager.UseResources(_settings.earthCost, _settings.waterCost);
                    GameObject building = GameObject.Instantiate(_settings.buildingPrefab, tile.transform.position, Quaternion.identity);
                    tile.InteractableBase = building.GetComponent<InteractableBase>();
                }
                else
                {
                    Debug.Log("There is already a building on this tile");
                }
            }
            else
            {
                Debug.Log("Not enough resources!");
            }
        }
        else
        {
            Debug.Log("Has to be build on Dirt!");
        }
        
    }

    public void OnSelection(Tile tile)
    {

    }
}