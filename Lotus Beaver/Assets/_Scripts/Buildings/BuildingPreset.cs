using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Building Preset", menuName = "ScriptableObjects/New Building Preset")]
public class BuildingPreset : ScriptableObject
{
    [field:SerializeField] public string displayName { get; private set; }

    [field: SerializeField] public int earthCost { get; private set; }
    [field: SerializeField] public int waterCost { get; private set; }

    [field: SerializeField] public GameObject buildingPrefab { get; private set; }

    [field: Header("Resource Production")]
    [field: SerializeField] public int earthProduction { get; private set; }
    [field: SerializeField] public int waterProduction { get; private set; }

    [field: SerializeField] public Sprite InteractionIcon;

    [field: Header("Healing")]
    [field: SerializeField] public int selfHealAmount { get; private set; }
    [field: SerializeField] public int healAmount { get; private set; }
    [field: SerializeField] public int healRadius { get; private set; }

    [field: Header("Construction")]
    [field: SerializeField] public int constructionStages { get; private set; }
    [field: SerializeField] public Sprite[] constructionSprites { get; private set; }
    [field: SerializeField] public int ticksPerStage { get; private set; }

    [field: Header("Upgrades")]
    [field:SerializeField] public int upgradeStages { get; private set; }
    [field: SerializeField] public Sprite[] upgradeSprites { get; private set; }
    [field: SerializeField] public int[] upgradeEarthCosts { get; private set; }
    [field: SerializeField] public int[] upgradeWaterCosts { get; private set; }





}
