using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TilePool))]
public class TileManager : MonoBehaviour
{
    private static Dictionary<TileElementType, Pool<TileElement>> _tileElementPools = new Dictionary<TileElementType, Pool<TileElement>>();

    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private Vector3 _correctionValue;

    private TilePool _tilePool;

    private static TileManager _instance;

    public static Tile[,] _tiles;

    public static Tile[,] Tiles => _tiles;

    private static int GetTilesMaxX()
    {
        return _tiles.GetLength(0) - 1;
    }

    private static int GetTilesMaxY()
    {
        return _tiles.GetLength(1) - 1;
    }

    private static int GetTilesXLength()
    {
        return _tiles.GetLength(0);
    }

    private static int GetTilesYLength()
    {
        return _tiles.GetLength(1);
    }

    private static Vector3 CenterOffset()
    {
        return new Vector3(
            -0.5f * (_instance._gameSettings.MapSize.x * _instance._gameSettings.TileSize.x),
            -0.5f * (_instance._gameSettings.MapSize.y * _instance._gameSettings.TileSize.y),
            0f)
            - HalfTileOffset();
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
        return ClampTile(ApplyCenterOffset(GetCoordinates(position)));
    }

    public static Tile GetClosetTile(Vector2Int coordinates)
    {
        return ClampTile(ApplyCenterOffset(coordinates));
    }

    private static Vector2Int ApplyCenterOffset(Vector2Int coordinates)
    {
        return coordinates - CenterTile();
    }

    private static Tile ClampTile(Vector2Int coordinates)
    {
        return _tiles[Math.Clamp(coordinates.x, 0, GetTilesMaxX()), Math.Clamp(coordinates.y, 0, GetTilesMaxY())];
    }

    private static Tile FilterOutOfBoundsCoordinates(Vector2Int coordinates)
    {
        if (coordinates.x < 0 || coordinates.x > GetTilesMaxX() || coordinates.y < 0 || coordinates.y > GetTilesMaxY())
        {
            return null;
        }

        return _tiles[coordinates.x, coordinates.y];
    }

    public static Vector2Int GetCoordinates(Vector3 position)
    {
        position.x /= _instance._gameSettings.TileSize.x;
        position.y /= _instance._gameSettings.TileSize.y;
        return new Vector2Int((int)Mathf.Round(position.x), (int)Mathf.Round(position.y)) + CenterTile();
    }

    public static Vector2Int CenterTile()
    {
        return new Vector2Int(GetTilesXLength() / 2, GetTilesYLength() / 2);
    }

    private Vector3 GetTilePosition(int x, int y)
    {
        return new Vector3(x * _gameSettings.TileSize.x, y * _gameSettings.TileSize.y, _gameSettings.TileHeight) + CenterOffset();
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

        for (int x = 0; x < _gameSettings.MapSize.x; x++)
        {
            for (int y = 0; y < _gameSettings.MapSize.y; y++)
            {
                Tile tile = _tilePool.GetPoolable();
                tile.Transform.position = GetTilePosition(x, y);
                tile.Transform.name = $"Tile {x},{y}";
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
            SetTileElementType(tile, TileElementType.Water);
        }

        Vector2Int center = new Vector2Int(GetTilesXLength() / 2, GetTilesYLength() / 2);

        SetTileElementType(_tiles[center.x, center.y], TileElementType.Earth);
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

    public static void SetTileElementType(Tile tile, TileElementType tileElementType)
    {
        tile.TileElement?.SetActive(false);
        TileElement tileElement = GetTileElement(tileElementType);
        tileElement.Transform.SetParent(tile.Transform);
        tileElement.Transform.localPosition = Vector3.zero;
        tile.TileElement = tileElement;
    }

    internal static List<Tile> GetSouroundingTiles(Tile currentTile)
    {
        List<Tile> result = new List<Tile>();

        AddTile(currentTile.Coordinates + Vector2Int.left, result);
        AddTile(currentTile.Coordinates + Vector2Int.right, result);
        AddTile(currentTile.Coordinates + Vector2Int.up, result);
        AddTile(currentTile.Coordinates + Vector2Int.down, result);

        return result;
    }

    private static void AddTile(Vector2Int coordinates, List<Tile> result)
    {
        Tile tile = FilterOutOfBoundsCoordinates(coordinates);

        if (tile != null)
        {
            result.Add(tile);
        }
    }
}