using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RootManager : MonoBehaviour
{
    [SerializeField] private Vector2Int[] _initalRootExtraFilds;
    [SerializeField] private Vector2Int[] _growthDirection;
    [SerializeField] private Vector2Int[] _rootStartOffset;
    private Vector2Int[] _rootStart;

    [SerializeField] private Sprite[] _middle;
    [SerializeField] private Sprite[] _start;
    [SerializeField] private Sprite[] _end;

    [SerializeField] private BuildingPreset _lotusBuildingPreset;

    private static int _rootLevel;

    private static RootManager _instance;
    public Building LotusBuilding { get; private set; }
    public static Building Lotus => _instance.LotusBuilding;
    public static BuildingPreset LotusBuildingPreset => _instance._lotusBuildingPreset;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        GameManager.OnNewGame += OnNewGame;
    }
    private void OnDestroy()
    {
        GameManager.OnNewGame -= OnNewGame;
    }

    private void OnNewGame()
    {
        _rootLevel = GameSettingsManager.GameSettings().RootLevel;
        GrowRoots();
    }
    private void GrowRoots()
    {
        Tile centerTile = TileManager.GetCenterTile();
        List<Tile> rootTiles = new() { centerTile };

        rootTiles.AddRange(TileManager.GetSurroundingTilesWithDiagonal(centerTile));

        LotusBuilding = Instantiate(_lotusBuildingPreset.BuildingPrefab, centerTile.transform);
        LotusBuilding.SpriteRenderer.sortingOrder = 1;

        for (int i = 0; i < _initalRootExtraFilds.Length; i++)
        {
            TileManager.AddTile(centerTile.Coordinates + _initalRootExtraFilds[i], rootTiles);
        }
        for (int i = 0; i < rootTiles.Count; i++)
        {
            TileManager.SetTileElementType(rootTiles[i], TileElementType.Root, out _);
        }
        for (int i = 0; i < rootTiles.Count; i++)
        {
            rootTiles[i].Building = LotusBuilding;
        }

        _rootStart = new Vector2Int[_rootStartOffset.Length];

        LotusBuilding.Upgrade();
    }

    public static void IncreaseRootLevel()
    {
        _rootLevel++;
        _instance.GrowOutRoots();
    }

    private void GrowOutRoots()
    {
        Tile centerTile = TileManager.GetCenterTile();
        for (int i = 0; i < _rootStartOffset.Length; i++)
        {
            _rootStart[i] = centerTile.Coordinates + _rootStartOffset[i];

            for (int j = 0; j < _rootLevel; j++)
            {
                Tile rootTile = TileManager.FilterOutOfBoundsCoordinates(_rootStart[i] + (_growthDirection[i] * j));

                TileManager.SetTileElementType(rootTile, TileElementType.Root, out _);

                if (j == 0)
                {
                    rootTile.TileElement.SpriteRenderer.sprite = _start[i];

                    continue;
                }
                else if (j == _rootLevel - 1)
                {
                    rootTile.TileElement.SpriteRenderer.sprite = _end[i];

                    continue;
                }

                rootTile.TileElement.SpriteRenderer.sprite = _middle[i];
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu(nameof(TestGrowRoots))]
    public void TestGrowRoots()
    {
        IncreaseRootLevel();
    }
#endif
}