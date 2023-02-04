using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : InteractableBase
{
    [SerializeField] private BuildingPreset buildingPreset;

    // Start is called before the first frame update
    void Start()
    {
        TickManager.OnBuildingTick += TickManager_OnBuildingTick;
    }

    private void TickManager_OnBuildingTick(object sender, TickManager.OnTickEventArgs e)
    {
        RessourceManager.AddEarth(buildingPreset.earthProduction);
        RessourceManager.AddWater(buildingPreset.waterProduction);
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
