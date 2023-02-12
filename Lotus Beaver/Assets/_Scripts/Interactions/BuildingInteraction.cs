using UnityEngine;

public class BuildingInteraction : Interactable
{
    public BuildingPreset BuildingPreset { get; set; }

    public BuildingInteraction(InteractionPreset interactionPreset, InteractionController interactionController, int index) : base(interactionPreset, interactionController, index)
    {
    }

    ~BuildingInteraction()
    {
        InteractionController.OnInteractionChange -= OnSelectionChange;
    }

    public override void OnInteract(Tile tile)
    {
        if (tile.TileElement.TileElementType == TileElementType.Earth)
        {
            if (tile.Building == null)
            {
                if (BuildingPreset != RootManager.LotusBuildingPreset)
                {
                    if (RessourceManager.EnoughResources(InteractionPreset.EarthCost, InteractionPreset.WaterCost))
                    {
                        RessourceManager.UseResources(InteractionPreset.EarthCost, InteractionPreset.WaterCost);

                        Building building = Object.Instantiate(BuildingPreset.BuildingPrefab, tile.Transform.position, Quaternion.identity, tile.Transform);

                        building.SetTile(tile);

                        tile.Building = building;

                        building.SpriteRenderer.sortingOrder = TileManager.GetSortOrderFromCoordinate(tile.Coordinates);

                        FlowerCreationPool.ParticlePool?.GetPoolable()?.Play(tile.Transform.position);

                        SoundManager.PlayPlanting();
#if UNITY_EDITOR
                        Debug.Log(building.BuildingName);
                        Debug.Log("New Sorting order: " + building.SpriteRenderer.sortingOrder);
#endif
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.Log("Not enough resources!");
#endif
                        SoundManager.PlayError();
                    }
                }
            }
            else if (tile.Building.BuildingPreset == BuildingPreset)
            {
                TryUpgrading(tile);
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log(tile.Building.BuildingName);
                Debug.Log(InteractionPreset.DisplayName);
                Debug.Log("There is already a building on this tile");
#endif
                SoundManager.PlayError();
            }
        }
        else
        {
            // check for root ground
            if (tile.TileElement.TileElementType == TileElementType.Root)
            {
                // check if lotus is selected and hovered
                if (tile.Building?.BuildingPreset == BuildingPreset)
                {
                    TryUpgrading(tile);
                }
                else
                {
                    SoundManager.PlayError();
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("Has to be build on Dirt!");
#endif
                SoundManager.PlayError();
            }
        }
    }

    private void TryUpgrading(Tile tile)
    {
        int currentUpgradeStage = tile.Building._currentUpgradeStage;
        if (currentUpgradeStage < tile.Building.UpgradeStages)
        {
            if (RessourceManager.EnoughResources(BuildingPreset.UpgradeStages[currentUpgradeStage].UpgradeEarthCosts, BuildingPreset.UpgradeStages[currentUpgradeStage].UpgradeWaterCosts))
            {
                RessourceManager.UseResources(BuildingPreset.UpgradeStages[currentUpgradeStage].UpgradeEarthCosts, BuildingPreset.UpgradeStages[currentUpgradeStage].UpgradeWaterCosts);
                tile.Building.Upgrade();
                SoundManager.PlayUpgrade();
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("Not enough resources to upgrade!");
#endif
                SoundManager.PlayError();
            }
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Max Upgrade Reached!");
#endif
            SoundManager.PlayError();
        }
    }

    public override void OnSelection(Tile tile)
    {
    }

    public bool CanBePlaced(Tile tile)
    {
        return tile.Building == null && RessourceManager.EnoughResources(InteractionPreset.EarthCost, InteractionPreset.WaterCost);
    }

    public override void OnSelectionChange(int selectedIndex)
    {
        if (selectedIndex == _index)
        {
            _interactionController.BuildPreviewSpriteRenderer.sprite = BuildingPreset.BuildPreview;
        }
    }
}