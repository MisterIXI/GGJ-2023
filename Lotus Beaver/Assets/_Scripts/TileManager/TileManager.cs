using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TilePool))]
public class TileManager : MonoBehaviour
{
    private static readonly Dictionary<TileElementType, Pool<TileElement>> _tileElementPools = new();

    [SerializeField] private GameSettings _gameSettings;

    private TilePool _tilePool;

    private static TileManager _instance;

    public static Tile[,] _tiles;

    public static Tile[,] Tiles => _tiles;

    public static int GetTilesMaxX()
    {
        return GetTilesXLength() - 1;
    }

    public static int GetTilesMaxY()
    {
        return GetTilesYLength() - 1;
    }

    public static int GetTilesXLength()
    {
        return _tiles.GetLength(0);
    }

    public static int GetTilesYLength()
    {
        return _tiles.GetLength(1);
    }

    public static Vector2Int GetSize()
    {
        return new Vector2Int(GetTilesXLength(), GetTilesYLength());
    }

    private static Vector3 CenterOffset()
    {
        return new Vector3(
            -0.5f * (GetTilesXLength() * GetTileSizeX()),
            -0.5f * (GetTilesYLength() * GetTileSizeY()),
            0f)
            - HalfTileOffset();
    }

    private static float GetTileSizeY()
    {
        return _instance._gameSettings.TileSize.y;
    }

    private static float GetTileSizeX()
    {
        return _instance._gameSettings.TileSize.x;
    }

    private static Vector3 HalfTileOffset()
    {
        return new Vector3(
            HalfTileOffsetX(),
            HalfTileOffsetY(),
            0f);
    }

    private static float HalfTileOffsetY()
    {
        return -0.5f * _instance._gameSettings.TileSize.y;
    }

    private static float HalfTileOffsetX()
    {
        return -0.5f * _instance._gameSettings.TileSize.x;
    }

    public static Tile GetClosetTile(Vector3 position)
    {
        return _tiles == null || _tiles.Length == 0 ? null : ClampTile(GetCoordinates(position));
    }

    public static bool IsOutOfBounds(Tile tile)
    {
        return tile.Coordinates.x <= 0
        || tile.Coordinates.x >= GetTilesMaxX()
        || tile.Coordinates.y <= 0
        || tile.Coordinates.y >= GetTilesMaxY();
    }

    public static Tile GetClosetTile(Vector2Int coordinates)
    {
        return _tiles == null || _tiles.Length == 0 ? null : ClampTile(coordinates);
    }

    private static Vector2Int ApplyCenterOffset(Vector2Int coordinates)
    {
        return coordinates - CenterTile();
    }

    private static Tile ClampTile(Vector2Int coordinates)
    {
        return _tiles[Math.Clamp(coordinates.x, 0, GetTilesMaxX()), Math.Clamp(coordinates.y, 0, GetTilesMaxY())];
    }

    public static Tile FilterOutOfBoundsCoordinates(Vector2Int coordinates)
    {
        return coordinates.x < 0 || coordinates.x > GetTilesMaxX() || coordinates.y < 0 || coordinates.y > GetTilesMaxY()
            ? null
            : _tiles[coordinates.x, coordinates.y];
    }

    public static int GetSortOrderFromPosition(Vector3 position, int offset = 0)
    {
        if (_tiles == null)
        {
            return 0;
        }

        int sortY = ((GetTilesYLength() - GetCoordinates(position).y) * GetTilesXLength()) + 2;
        int sortx = GetCoordinates(position).x;
        int result = sortx + sortY;
        if (offset > 0)
        {
            result = sortY + GetTilesXLength() + offset;
        }

        return result;
    }

    public static int GetSortOrderFromCoordinate(Vector2Int coordinate, int offset = 0)
    {
        if (_tiles == null)
        {
            return 0;
        }

        int sortY = ((GetTilesYLength() - coordinate.y) * GetTilesXLength()) + 2;
        int sortx = coordinate.x;
        int result = sortx + sortY;
        if (offset > 0)
        {
            result = sortY + GetTilesXLength() + offset;
        }

        return result;
    }

    public static Vector2Int GetCoordinates(Vector3 position)
    {
        position -= new Vector3(0.5f * GetTileSizeX(), 0.5f * GetTileSizeY(), 0);
        position.x /= GetTileSizeX();
        position.y /= GetTileSizeY();
        return new Vector2Int((int)Mathf.Round(position.x), (int)Mathf.Round(position.y)) + CenterTile();
    }

    public static Tile GetCenterTile()
    {
        return GetClosetTile(CenterTile());
    }

    public static Vector2Int CenterTile()
    {
        return new Vector2Int(GetTilesXLength() / 2, GetTilesYLength() / 2);
    }

    public static Vector2 CenterTileOffset()
    {
        return Vector2.zero;
        //return new Vector2(GetTilesXLength() / 2 * _instance._gameSettings.TileSize.x, GetTilesYLength() / 2 * _instance._gameSettings.TileSize.y);
    }

    private Vector3 GetTilePosition(int x, int y)
    {
        return new Vector3(x * GetTileSizeX(), y * GetTileSizeY(), GetTileHeight()) + CenterOffset();
    }

    private float GetTileHeight()
    {
        return _gameSettings.TileHeight;
    }

    public static void AddTileElementPool(TileElementType tileElementType, Pool<TileElement> pool)
    {
        _tileElementPools.Add(tileElementType, pool);
    }

    public static TileElement GetTileElement(TileElementType tileElementType)
    {
        if (_tileElementPools.TryGetValue(tileElementType, out Pool<TileElement> pool))
        {
            TileElement tileElement = pool.GetPoolable();
            tileElement.SetActive(true);
            return tileElement;
        }

        return null;
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        _tilePool = GetComponent<TilePool>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        CleanUpTiles();

        GenerateTiles();

        GenerateTileElements();
    }

    [ContextMenu(nameof(CleanUpTiles))]
    private void CleanUpTiles()
    {
        if (_tiles == null)
        {
            return;
        }

        foreach (Tile tile in GetNextTile())
        {
            tile?.SetActive(false);
        }
    }

    [ContextMenu(nameof(GenerateTiles))]
    private void GenerateTiles()
    {
        _tiles = new Tile[_gameSettings.MapSize.x, _gameSettings.MapSize.y];

        for (int x = 0; x < GetTilesXLength(); x++)
        {
            for (int y = 0; y < GetTilesYLength(); y++)
            {
                Tile tile = _tilePool.GetPoolable();
                tile.Transform.position = GetTilePosition(x, y);
#if UNITY_EDITOR
                tile.Transform.name = $"Tile {x},{y}";
#endif
                tile.Coordinates = new Vector2Int(x, y);
                tile.SetActive(true);
                _tiles[x, y] = tile;
            }
        }
    }

    [ContextMenu(nameof(CleanUpTileElements))]
    private void CleanUpTileElements()
    {
        if (_tiles == null)
        {
            return;
        }

        foreach (Tile tile in GetNextTile())
        {
            tile?.TileElement?.SetActive(false);
        }
    }

    [ContextMenu(nameof(GenerateTileElements))]
    public void GenerateTileElements()
    {
        foreach (Tile tile in GetNextTile())
        {
            SetTileElementType(tile, TileElementType.Water, out _);
        }

        Vector2Int center = new(GetTilesXLength() / 2, GetTilesYLength() / 2);

        SetTileElementType(_tiles[center.x, center.y], TileElementType.Earth, out _);
    }

    private static IEnumerable<Tile> GetNextTile()
    {
        for (int x = 0; x < GetTilesXLength(); x++)
        {
            for (int y = 0; y < GetTilesYLength(); y++)
            {
                yield return _tiles[x, y];
            }
        }
    }

    public static void SetTileElementType(Tile tile, TileElementType tileElementType, out TileController tileController)
    {

        tileController = null;
        if (tile == null)
        {
            return;
        }

        if (tile.Building != null && tileElementType != TileElementType.Earth)
        {
            Destroy(tile.Building.GameObject());
        }

        tile.TileElement?.SetActive(false);
        TileElement tileElement = GetTileElement(tileElementType);
        tile.TileElement = tileElement;
        tileController = tileElement.TileController;

        tileElement.Transform.SetParent(tile.Transform);
        tileElement.Transform.localPosition = Vector3.zero;

        UpdateCliffTiles(tile);
    }

    private static void UpdateCliffTiles(Tile tile)
    {
        foreach (Tile surroundingTile in GetSurroundingTilesWithDiagonal(tile))
        {
            bool surrByWater = IsSurroundedByWaterOrCliff(surroundingTile);

            TileController tileController = surroundingTile?.TileElement?.TileController;

            if (tileController is CliffController cliffController)
            {
                if (surrByWater)
                {
                    SetTileElementType(surroundingTile, TileElementType.Water, out _);
                }
                else
                {
                    cliffController.UpdateCliffSprite();
                }
            }
            else if (tileController is WaterController)
            {
                if (!surrByWater)
                {
                    SetTileElementType(surroundingTile, TileElementType.Cliff, out tileController);
                    if (tileController is CliffController surroundingTileCliff)
                    {
                        surroundingTileCliff.UpdateCliffSprite();
                    }
                }
            }
        }
    }

    public static List<Tile> GetSurroundingTiles(Tile currentTile)
    {
        List<Tile> result = new();

        AddTile(currentTile.Coordinates + Vector2Int.left, result);
        AddTile(currentTile.Coordinates + Vector2Int.right, result);
        AddTile(currentTile.Coordinates + Vector2Int.up, result);
        AddTile(currentTile.Coordinates + Vector2Int.down, result);

        return result;
    }

    public static List<Tile> GetSurroundingTilesWithDiagonal(Tile currentTile)
    {
        List<Tile> result = GetSurroundingTiles(currentTile);

        AddTile(currentTile.Coordinates + Vector2Int.left + Vector2Int.up, result);
        AddTile(currentTile.Coordinates + Vector2Int.right + Vector2Int.up, result);
        AddTile(currentTile.Coordinates + Vector2Int.left + Vector2Int.down, result);
        AddTile(currentTile.Coordinates + Vector2Int.right + Vector2Int.down, result);

        return result;
    }

    public static bool IsSurroundedByWaterOrCliff(Tile tile)
    {
        foreach (Tile surroundingTile in GetSurroundingTilesWithDiagonal(tile))
        {
            if (surroundingTile == null || surroundingTile.TileElement == null)
            {
                continue;
            }

            if (surroundingTile.TileElement.TileElementType is not TileElementType.Water and not TileElementType.Cliff)
            {
                return false;
            }
        }
        return true;
    }

    public static SurroundingTiles GetSurroundingTilesElements(Tile currentTile)
    {
        if (currentTile == null)
        {
            return new SurroundingTiles();
        }

        Vector2Int coordinates = currentTile.Coordinates;

        return new SurroundingTiles()
        {
            UpLeft = FilterOutOfBoundsCoordinates(coordinates + Vector2Int.up + Vector2Int.left)?.TileElement,
            Up = FilterOutOfBoundsCoordinates(coordinates + Vector2Int.up)?.TileElement,
            UpRight = FilterOutOfBoundsCoordinates(coordinates + Vector2Int.up + Vector2Int.right)?.TileElement,
            Right = FilterOutOfBoundsCoordinates(coordinates + Vector2Int.right)?.TileElement,
            DownRight = FilterOutOfBoundsCoordinates(coordinates + Vector2Int.down + Vector2Int.right)?.TileElement,
            Down = FilterOutOfBoundsCoordinates(coordinates + Vector2Int.down)?.TileElement,
            DownLeft = FilterOutOfBoundsCoordinates(coordinates + Vector2Int.down + Vector2Int.left)?.TileElement,
            Left = FilterOutOfBoundsCoordinates(coordinates + Vector2Int.left)?.TileElement
        };
    }

    public static void AddTile(Vector2Int coordinates, List<Tile> result)
    {
        Tile tile = FilterOutOfBoundsCoordinates(coordinates);

        if (tile != null)
        {
            result.Add(tile);
        }
    }
}