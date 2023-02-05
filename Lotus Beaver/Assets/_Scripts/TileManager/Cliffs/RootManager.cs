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

    private static int _rootLevel;

    private void Awake()
    {
        GameManager.OnNewGame += OnNewGame;
    }

    private void OnNewGame()
    {
        _rootLevel = GameSettingsManager.GameSettings().RootLevel;
        GrowRoots();
    }

    public void IncreaseRootLevel()
    {
        _rootLevel++;
        GrowRoots();
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

        for (int i = 0; i < _initalRootExtraFilds.Length; i++)
        {
            TileManager.AddTile(centerTile.Coordinates + _initalRootExtraFilds[i], rootTiles);
        }

        _rootStart = new Vector2Int[_rootStartOffset.Length];

        for (int i = 0; i < rootTiles.Count; i++)
        {
            TileManager.SetTileElementType(rootTiles[i], TileElementType.Root);
        }

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
                else if(j == _rootLevel - 1)
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
