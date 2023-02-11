using UnityEngine;

public class BuildingInteraction : IInteractable
{
    public BuildingPreset Settings { get; }
    private readonly InteractionController _interactionController;
    private readonly int _index;

    public BuildingInteraction(BuildingPreset settings, InteractionController interactionController, int index)
    {
        Settings = settings;
        _interactionController = interactionController;
        _index = index;
        InteractionController.OnInteractionChange += OnSelectionChange;
    }

    public void OnInteract(Tile tile)
    {
        // instantiate building
        if (tile.TileElement.TileElementType == TileElementType.Earth)
        {
            if (tile.Building == null)
            {
                if (Settings.DisplayName != "Lotus")
                {
                    if (RessourceManager.EnoughResources(Settings.EarthCost, Settings.WaterCost))
                    {
                        RessourceManager.UseResources(Settings.EarthCost, Settings.WaterCost);

                        Building building = Object.Instantiate(Settings.BuildingPrefab, tile.Transform.position, Quaternion.identity, tile.Transform);

                        building.SetTile(tile);

                        FlowerCreationPool.ParticlePool?.GetPoolable()?.Play(tile.Transform.position);

                        SoundManager.PlayPlanting();

                        Debug.Log(building.BuildingName);

                        tile.Building = building;
                        SpriteRenderer spriteRenderer = building.GetComponentInChildren<SpriteRenderer>();

                        spriteRenderer.sortingOrder = TileManager.GetSortOrderFromPosition(building.transform.position);
#if UNITY_EDITOR
                        Debug.Log("New Sorting order: " + spriteRenderer.sortingOrder);
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
            else if (tile.Building.BuildingName == Settings.DisplayName)
            {
                TryUpgrading(tile);
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log(tile.Building.BuildingName);
                Debug.Log(Settings.DisplayName);
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
                if (tile.Building?.BuildingName == Settings.DisplayName)
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
                Debug.Log("Has to be build on Dirt!");
                SoundManager.PlayError();
            }
        }
    }

    private void TryUpgrading(Tile tile)
    {
        int currentUpgradeStage = tile.Building.currentUpgradeStage;
        if (currentUpgradeStage < tile.Building.UpgradeStages)
        {
            if (RessourceManager.EnoughResources(Settings.UpgradeStages[currentUpgradeStage].UpgradeEarthCosts, Settings.UpgradeStages[currentUpgradeStage].UpgradeWaterCosts))
            {
                RessourceManager.UseResources(Settings.UpgradeStages[currentUpgradeStage].UpgradeEarthCosts, Settings.UpgradeStages[currentUpgradeStage].UpgradeWaterCosts);
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

    public void OnSelection(Tile tile)
    {
    }

    public bool CanBePlaced(Tile tile)
    {
        return tile.Building == null && RessourceManager.EnoughResources(Settings.EarthCost, Settings.WaterCost);
    }

    public void OnSelectionChange(int selectedIndex)
    {
        if (selectedIndex == _index)
        {
            _interactionController.BuildPreviewSpriteRenderer.sprite = Settings.BuildPreview;
        }
    }
}