using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    [field: Header("Map Settings")]
    [field: SerializeField] public Vector2Int MapSize { get; private set; }
    [field: SerializeField] public Vector2 TileSize { get; private set; }
    [field: SerializeField] public float TileHeight { get; private set; }

    [field: Header("Tick Settings")]
    [field: SerializeField] public float DamageTickTime { get; private set; }
    [field: SerializeField] public float BuildingTickTime { get; private set; }
    [field: SerializeField] public float ConstructionTickTime { get; private set; }

    [field: Header("Root Settings")]
    [field: SerializeField] public int RootLevel{ get; private set; }
}