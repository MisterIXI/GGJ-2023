using System;
using UnityEngine;

[Serializable]
public class ConstructionStage
{
    [SerializeField] private Sprite _sprite;

    public Sprite Sprite => _sprite;
}