using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : InteractableBase
{
    [SerializeField] private BuildingPreset buildingPreset;

    public string buildingName;

    private int selfHealAmount;
    private int healAmount;
    private int healRadius;

    private bool isBuild = false;

    private int constructionTicks = 0;
    private int ticksPerStage;

    private int constructionStages;
    private int currentConstructionStage = 0;


    private Sprite[] upgradeSprites;
    public int upgradeStages;
    public int currentUpgradeStage = 0;
    private int[] upgradeEarthCosts;
    private int[] upgradeWaterCosts;

    private Sprite[] sprites;


    // Start is called before the first frame update
    void Start()
    {
        TickManager.OnBuildingTick += TickManager_OnBuildingTick;
        TickManager.OnConstructionTick += TickManager_OnConstructionTick;
        TickManager.OnDamageTick += TickManager_OnDamageTick;

        buildingName = buildingPreset.displayName;

        constructionStages = buildingPreset.constructionStages;
        ticksPerStage = buildingPreset.ticksPerStage;
        sprites = buildingPreset.constructionSprites;

        selfHealAmount = buildingPreset.selfHealAmount;
        healAmount = buildingPreset.healAmount;
        healRadius = buildingPreset.healRadius;

        upgradeSprites = buildingPreset.upgradeSprites;
        upgradeStages = buildingPreset.upgradeStages;
        upgradeEarthCosts = buildingPreset.upgradeEarthCosts;
        upgradeWaterCosts = buildingPreset.upgradeWaterCosts;

        if (constructionStages > 0)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = sprites[0];
        }
        else
        {
            isBuild = true;
        }
    }



    private void TickManager_OnBuildingTick(object sender, TickManager.OnTickEventArgs e)
    {
        if (isBuild)
        {
            RessourceManager.AddEarth(buildingPreset.earthProduction);
            RessourceManager.AddWater(buildingPreset.waterProduction);
        }
    }

    private void TickManager_OnConstructionTick(object sender, TickManager.OnTickEventArgs e)
    {
        if (!isBuild && currentConstructionStage < constructionStages)
        {
            constructionTicks++;
            if (constructionTicks >= ticksPerStage)
            {
                currentConstructionStage++;
                constructionTicks = 0;
                GetComponentInChildren<SpriteRenderer>().sprite = sprites[currentConstructionStage];

                if (currentConstructionStage == constructionStages)
                {
                    isBuild = true;

                    Animator anim = GetComponentInChildren<Animator>();
                    if (anim != null)
                    {
                        anim.enabled = true;
                        //anim.SetBool("isBuild", true);
                    }
                }

            }
        }
    }

    private void TickManager_OnDamageTick(object sender, TickManager.OnTickEventArgs e)
    {
        Tile currentTile = GetComponentInParent<Tile>();
        currentTile.TileElement.GetComponent<EarthController>().GetHealth(selfHealAmount);
        if (healRadius > 0)
        {
            foreach (var tile in TileManager.GetSurroundingTilesWithDiagonal(currentTile))
            {
                if (tile.TileElement.TileElementType == TileElementType.Earth)
                {
                    tile.TileElement.GetComponent<EarthController>().GetHealth(healAmount);
                }

            }
        }
    }

    public override void OnInteract(Tile tile)
    {
        Debug.Log("Interacted with " + tile);
    }

    public override void OnSelection(Tile tile)
    {
        Debug.Log("Selected " + tile);
    }

    public void Upgrade()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = upgradeSprites[currentUpgradeStage];
        currentUpgradeStage++;
    }
}
