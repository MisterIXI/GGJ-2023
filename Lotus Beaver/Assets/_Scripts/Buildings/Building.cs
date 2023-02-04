using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : InteractableBase
{
    [SerializeField] private BuildingPreset buildingPreset;

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

        constructionStages = buildingPreset.constructionStages;
        ticksPerStage = buildingPreset.ticksPerStage;
        sprites = buildingPreset.sprites;

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

    public override void OnInteract(Tile tile)
    {   
        Debug.Log("Interacted with " + tile);
    }

    public override void OnSelection(Tile tile)
    {
        Debug.Log("Selected " + tile);
    }
}
