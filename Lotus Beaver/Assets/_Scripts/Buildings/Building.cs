using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : InteractableBase
{
    [SerializeField] private BuildingPreset buildingPreset;

    private int selfHealAmount;
    private int healAmount;
    private int healRadius;

    private bool isBuild = false;

    private int constructionTicks = 0;
    private int ticksPerStage;

    private int constructionStages;
    private int currentStage = 0;

    Sprite[] sprites;


    // Start is called before the first frame update
    void Start()
    {
        TickManager.OnBuildingTick += TickManager_OnBuildingTick;
        TickManager.OnConstructionTick += TickManager_OnConstructionTick;
        TickManager.OnDamageTick += TickManager_OnDamageTick;

        constructionStages = buildingPreset.constructionStages;
        ticksPerStage = buildingPreset.ticksPerStage;
        sprites = buildingPreset.sprites;

        selfHealAmount = buildingPreset.selfHealAmount;
        healAmount = buildingPreset.healAmount;
        healRadius = buildingPreset.healRadius;

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
        if (!isBuild && currentStage < constructionStages)
        {
            constructionTicks++;
            if (constructionTicks >= ticksPerStage)
            {
                currentStage++;
                constructionTicks = 0;
                GetComponentInChildren<SpriteRenderer>().sprite = sprites[currentStage];

                if (currentStage == constructionStages)
                    isBuild = true;
            }
        } 
    }

    private void TickManager_OnDamageTick(object sender, TickManager.OnTickEventArgs e)
    {
        Tile currentTile = GetComponentInParent<Tile>();
        //currentTile.Heal(selfHealAmount);
        if (healRadius > 0)
        {
            foreach(var tile in TileManager.GetSouroundingTilesWithDiagonal(currentTile))
            {
                if(tile.TileElement.TileElementType == TileElementType.Earth)
                {
                    //tile.Heal(healAmount);
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
}
