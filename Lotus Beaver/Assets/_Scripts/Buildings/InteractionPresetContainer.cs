using UnityEngine;

[CreateAssetMenu(fileName = "Interaction Preset Container ", menuName = "ScriptableObjects/New Interaction Preset Container ")]
public class InteractionPresetContainer : ScriptableObject
{
    [field: SerializeField] public InteractionPreset InteractionPreset { get; private set; }

    public Sprite InteractionIcon => InteractionPreset.InteractionIcon;
    public string DisplayName => InteractionPreset.DisplayName;
    public string Description => InteractionPreset.Description;
    public float EarthCost => InteractionPreset.EarthCost;
    public float WaterCost => InteractionPreset.WaterCost;
}
