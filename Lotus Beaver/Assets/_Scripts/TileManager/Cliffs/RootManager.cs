﻿using System.Collections.Generic;
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

    [SerializeField] private GameObject _lotusPrefab;

    private static int _rootLevel;

    private static RootManager _instance;
    public Building LotusBuilding { get; private set; }
    public static Building Lotus => _instance.LotusBuilding;

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

    public static void IncreaseRootLevel()
    {
        _rootLevel++;
        _instance.GrowOutRoots();
    }

    [ContextMenu(nameof(TestGrowRoots))]
    public void TestGrowRoots()
    {
        IncreaseRootLevel();
    }

    private void GrowRoots()
    {
        Tile centerTile = TileManager.GetCenterTile();
        List<Tile> rootTiles = new List<Tile>() { centerTile };

        rootTiles.AddRange(TileManager.GetSurroundingTilesWithDiagonal(centerTile));

        Building lotus = Instantiate(_lotusPrefab, centerTile.transform).GetComponent<Building>();
        lotus.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
        LotusBuilding = lotus;
        for (int i = 0; i < _initalRootExtraFilds.Length; i++)
        {
            TileManager.AddTile(centerTile.Coordinates + _initalRootExtraFilds[i], rootTiles);
        }
        for (int i = 0; i < rootTiles.Count; i++)
        {
            TileManager.SetTileElementType(rootTiles[i], TileElementType.Root);
        }
        for (int i = 0; i < rootTiles.Count; i++)
        {
            rootTiles[i].building = lotus;
        }

        _rootStart = new Vector2Int[_rootStartOffset.Length];

        GrowOutRoots();
    }

    private void GrowOutRoots()
    {
        Tile centerTile = TileManager.GetCenterTile();
        for (int i = 0; i < _rootStartOffset.Length; i++)
        {
            _rootStart[i] = centerTile.Coordinates + _rootStartOffset[i];

            for (int j = 0; j < _rootLevel; j++)
            {
                Tile rootTile = TileManager.FilterOutOfBoundsCoordinates(_rootStart[i] + _growthDirection[i] * j);

                TileManager.SetTileElementType(rootTile, TileElementType.Root);

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
    public class CustomArray<T>
    {
        public T[] GetColumn(T[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                    .Select(x => matrix[x, columnNumber])
                    .ToArray();
        }

        public T[] GetRow(T[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }
    }
}
