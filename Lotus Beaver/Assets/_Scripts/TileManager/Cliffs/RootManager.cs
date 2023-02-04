using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RootManager : MonoBehaviour
{

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

    public static void IncreaseRootLevel()
    {
        _rootLevel++;
        GrowRoots();
    }

    [ContextMenu(nameof(TestGrowRoots))]
    public void TestGrowRoots()
    {
        IncreaseRootLevel();
    }

    private static void GrowRoots()
    {
        Vector2Int cornerOffSet = TileManager.GetSize();
        cornerOffSet.x /= 2;
        cornerOffSet.y /= 2;
        cornerOffSet.x -= _rootLevel;
        cornerOffSet.y -= _rootLevel;

        Vector2Int centerCoordinates = TileManager.CenterTile();

        List<Tile> xTiles = Enumerable.Range(cornerOffSet.x, _rootLevel * 2 + 1).Select(x => TileManager.Tiles[x, centerCoordinates.y]).ToList();
        List<Tile> yTiles = Enumerable.Range(cornerOffSet.y, _rootLevel * 2 + 1).Select(x => TileManager.Tiles[centerCoordinates.y, x]).ToList();

        for (int x = 0; x < xTiles.Count; x++)
        {
            if (x == xTiles.Count / 2)
            {
                continue;
            }

            TileManager.SetTileElementType(xTiles[x], TileElementType.Root);
        }

        for (int y = 0; y < yTiles.Count;  y++)
        {
            if (y == xTiles.Count / 2)
            {
                continue;
            }

            TileManager.SetTileElementType(yTiles[y], TileElementType.Root);
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
