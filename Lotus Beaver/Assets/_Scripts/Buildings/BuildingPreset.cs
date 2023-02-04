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

    [field: SerializeField] public int earthProduction { get; private set; }
    [field: SerializeField] public int waterProduction { get; private set; }

    [field: SerializeField] public Sprite InteractionIcon;
}