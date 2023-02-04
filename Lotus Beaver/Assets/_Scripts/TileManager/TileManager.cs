using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(TilePool))]
public class TileManager : MonoBehaviour
{
    private static Dictionary<TileElementType, Pool<TileElement>> _tileElementPools = new Dictionary<TileElementType, Pool<TileElement>>();

    [SerializeField] private GameSettings _gameSettings;

    private TilePool _tilePool;

    private static TileManager _instance;

    public static Tile[,] _tiles;

    public static Tile[,] Tiles => _tiles;

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
                tile.TilePosition = new Vector2Int(x, y);
                tile.SetActive(true);
                _tiles[x, y] = tile;
            }
        }
    }

    private Vector3 GetTilePosition(int x, int y)
    {
        return new Vector3(x * _gameSettings.TileSize.x, y * _gameSettings.TileSize.y, _gameSettings.TileHeight) + CenterOffset();
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
        return -0.5f * (1 * _instance._gameSettings.TileSize.y);
    }

    private static float HalfTileOffsetX()
    {
        return -0.5f * (1 * _instance._gameSettings.TileSize.x);
    }

    public static void AddTileElementPool(TileElementType tileElementType, Pool<TileElement> pool)
    {
        _tileElementPools.Add(tileElementType, pool);
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

        Vector2Int center = new Vector2Int(_tiles.GetLength(0) / 2, _tiles.GetLength(1) / 2);

        SetTileElementType(_tiles[center.x, center.y], TileElementType.Earth);
    }

    private static IEnumerable<Tile> GetNextTile()
    {
        for (int x = 0; x < _tiles.GetLength(0); x++)
        {
            for (int y = 0; y < _tiles.GetLength(1); y++)
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

    public static Tile GetClosetTile(Vector3 position)
    {
        Vector2Int coordinates = GetCoordinates(position);

        return ClampTile(coordinates);
    }

    public static Tile GetClosetTile(Vector2Int coordinates)
    {
        return ClampTile(coordinates);
    }

    public static Tile ClampTile(Vector2Int coordinates)
    {
        coordinates -= CenterTile();

        return _tiles[Math.Clamp(coordinates.x, 0, GetTilesMaxX()), Math.Clamp(coordinates.y, 0, GetTilesMaxY())];
    }

    public static Vector2Int GetCoordinates(Vector3 position)
    {
        return new Vector2Int((int)Mathf.Round(position.x), (int)Mathf.Round(position.y)) + CenterTile();
    }

    public static Vector2Int CenterTile()
    {
        return new Vector2Int(GetTilesXLength() / 2, GetTilesYLength() / 2);
    }

    public static int GetTilesMaxX()
    {
        return _tiles.GetLength(0) - 1;
    }

    public static int GetTilesMaxY ()
    {
        return _tiles.GetLength(1) - 1;
    }

    public static int GetTilesXLength()
    {
        return _tiles.GetLength(0);
    }

    public static int GetTilesYLength()
    {
        return _tiles.GetLength(1);
    }
}
