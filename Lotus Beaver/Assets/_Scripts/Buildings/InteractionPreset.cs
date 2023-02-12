using System;
using UnityEngine;

[Serializable]
public class InteractionPreset
{

    [field: SerializeField] public Sprite InteractionIcon;
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public float EarthCost { get; private set; }
    [field: SerializeField] public float WaterCost { get; private set; }
}