using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
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
}
