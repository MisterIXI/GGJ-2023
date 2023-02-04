using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    [field: SerializeField] public Vector2Int MapSize { get; private set; }
    [field: SerializeField] public Vector2 TileSize { get; private set; }
    [field: SerializeField] public float TileHeight { get; private set; }
}