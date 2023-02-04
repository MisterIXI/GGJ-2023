using UnityEngine;

[CreateAssetMenu(fileName = "BuildingInteractionSettings", menuName = "Lotus Beaver/BuildingInteractionSettings", order = 0)]
public class BuildingInteractionSettings : ScriptableObject {
    public int earthCost = 0;
    public int waterCost = 0;
    // public Building building;
    public GameObject buildingPrefab;
}