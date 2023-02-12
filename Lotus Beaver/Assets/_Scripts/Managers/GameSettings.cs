using System.Linq;
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

    [field: Header("Earth Settings")]
    [field: SerializeField] public int EarthPlacementCost { get; private set; }

    [field: Header("Root Settings")]
    [field: SerializeField] public int RootLevel { get; private set; }

    [field: SerializeField] public int RootGrowthPerUpgrade { get; private set; }

    [field: Header("Start Resources")]
    [field: SerializeField] public float startEarth { get; private set; }

    [field: SerializeField] public float startWater { get; private set; }

#if UNITY_EDITOR
    [ContextMenu(nameof(SetTilePools))]
    private void SetTilePools()
    {
        int size = MapSize.x * MapSize.y;
        FindObjectOfType<TilePool>()?.SetInitialSize(size);
        TileElementPool[] pools = FindObjectsOfType<TileElementPool>();

        pools.FirstOrDefault(x => x.name == "WaterTileElementPool")?.SetInitialSize(size);
        pools.FirstOrDefault(x => x.name == "EarthTileElementPool")?.SetInitialSize(size);
        pools.FirstOrDefault(x => x.name == "CliffTileElementPool")?.SetInitialSize(size);
        pools.FirstOrDefault(x => x.name == "RootTileElementPool")?.SetInitialSize(MapSize.x + MapSize.y + 17);
    }
#endif
}