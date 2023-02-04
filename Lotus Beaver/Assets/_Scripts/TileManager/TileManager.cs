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
        return new Vector3(x * _gameSettings.TileSize.x, y * _gameSettings.TileSize.y, _gameSettings.TileHeight);
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
}
