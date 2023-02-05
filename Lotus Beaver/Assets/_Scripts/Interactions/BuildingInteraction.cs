using UnityEngine;

using System;
using System.Resources;

public class BuildingInteraction : IInteractable
{
    private BuildingPreset _settings;
    private InteractionController _interactionController;
    private int _index;
    public BuildingInteraction(BuildingPreset settings, InteractionController interactionController, int index)
    {
        _settings = settings;
        _interactionController = interactionController;
        _index = index;
        InteractionController.OnInteractionChange += OnSelectionChange;
    }

    public void OnInteract(Tile tile)
    {
        // instantiate building
        if (tile.TileElement.TileElementType == TileElementType.Earth)
        {
            if (tile.building == null)
            {
                if (_settings.displayName != "Lotus")
                {
                    if (RessourceManager.EnoughResources(_settings.earthCost, _settings.waterCost))
                    {
                        RessourceManager.UseResources(_settings.earthCost, _settings.waterCost);
                        GameObject building = GameObject.Instantiate(_settings.buildingPrefab, tile.transform.position, Quaternion.identity, tile.Transform);
                        SoundManager.PlayPlanting();
                        Debug.Log(building.GetComponent<Building>().buildingName);
                        tile.building = building.GetComponent<Building>();
                        SpriteRenderer spriteRenderer = building.GetComponentInChildren<SpriteRenderer>();
                        spriteRenderer.sortingOrder = TileManager.GetTilesMaxY() - TileManager.GetCoordinates(building.transform.position).y;
                        Debug.Log("New Sorting order: " + spriteRenderer.sortingOrder);
                    }
                    else
                    {
                        Debug.Log("Not enough resources!");
                        SoundManager.PlayError();
                    }
                }
            }
            else if (tile.building.buildingName == _settings.displayName)
            {
                TryUpgrading(tile);

            }
            else
            {
                Debug.Log(tile.building.buildingName);
                Debug.Log(_settings.displayName);
                Debug.Log("There is already a building on this tile");
                SoundManager.PlayError();
            }
        }
        else
        {
            // check for root ground
            if (tile.TileElement.TileElementType == TileElementType.Root)
            {
                // check if lotus is selected and hovered
                if (tile.building?.buildingName == _settings.displayName)
                    TryUpgrading(tile);
                else
                    SoundManager.PlayError();
            }
            else
            {
                Debug.Log("Has to be build on Dirt!");
                SoundManager.PlayError();
            }

        }

    }

    private void TryUpgrading(Tile tile)
    {
        int currentUpgradeStage = tile.building.currentUpgradeStage;
        if (currentUpgradeStage < tile.building.upgradeStages)
        {
            if (RessourceManager.EnoughResources(_settings.upgradeEarthCosts[currentUpgradeStage], _settings.upgradeWaterCosts[currentUpgradeStage]))
            {
                RessourceManager.UseResources(_settings.upgradeEarthCosts[currentUpgradeStage], _settings.upgradeWaterCosts[currentUpgradeStage]);
                tile.building.Upgrade();
                SoundManager.PlayUpgrade();
            }
            else
            {
                Debug.Log("Not enough resources to upgrade!");
                SoundManager.PlayError();
            }
        }
        else
        {
            Debug.Log("Max Upgrade Reached!");
            SoundManager.PlayError();
        }
    }

    public void OnSelection(Tile tile)
    {

    }

    public bool CanBePlaced(Tile tile)
    {
        if (tile.building != null)
            return false;
        if (!RessourceManager.EnoughResources(_settings.earthCost, _settings.waterCost))
            return false;
        return true;
    }
    public void OnSelectionChange(int selectedIndex)
    {
        if (selectedIndex == _index)
            _interactionController.BuildPreviewSpriteRenderer.sprite = _settings.BuildPreview;
    }
}