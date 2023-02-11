using UnityEngine;

[CreateAssetMenu(fileName = "Building Preset", menuName = "ScriptableObjects/New Building Preset")]
public class BuildingPreset : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    [field: SerializeField] public Sprite InteractionIcon;
    [field: SerializeField] public Sprite BuildPreview { get; private set; }
    [field: SerializeField] public Building BuildingPrefab { get; private set; }
    [field: SerializeField] public float EarthCost { get; private set; }
    [field: SerializeField] public float WaterCost { get; private set; }

    [field: Header("Resource Production")]
    [field: SerializeField] public float EarthProduction { get; private set; }
    [field: SerializeField] public float WaterProduction { get; private set; }


    [field: Header("Healing")]
    [field: SerializeField] public int SelfHealAmount { get; private set; }
    [field: SerializeField] public int HealAmount { get; private set; }
    [field: SerializeField] public int HealRadius { get; private set; }


    [field: Header("Construction")]
    [field: SerializeField] public ConstructionStage[] ConstructionStages { get; private set; }
    [field: SerializeField] public int TicksPerStage { get; private set; }
    public int ConstructionStagesLength => ConstructionStages.Length;


    [field: Header("Upgrades")]
    [field: SerializeField] public UpgradeStage[] UpgradeStages { get; private set; }
    public int UpgradeStagesLength => UpgradeStages.Length;
}
