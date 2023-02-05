using System;
using UnityEngine;

[Serializable]
public class DamageStage
{
    [SerializeField] private float _healthTreshHold;
    [SerializeField] private Sprite _sprite;

    public float HealthTreshHold => _healthTreshHold;
    public Sprite Sprite => _sprite;
}