using System;
using UnityEngine;

[Serializable]
public class UpgradeStage
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _upgradeEarthCost;
    [SerializeField] private float _upgradeWaterCost;

    public Sprite Sprite => _sprite;
    public float UpgradeEarthCosts => _upgradeEarthCost;
    public float UpgradeWaterCosts => _upgradeWaterCost;
}