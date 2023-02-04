using System.Collections.Generic;
using UnityEngine;

public class TileManagerTester : MonoBehaviour
{
    [SerializeField] private Tile _currentTile;
    [SerializeField] private List<Tile> _sourroundingTiles;
    [SerializeField] private Vector2Int _coordinates;

    private void Update()
    {
        if (TileManager.Tiles == null || TileManager.Tiles.Length == 0)
        {
            return;
        }

        _coordinates = TileManager.GetCoordinates(transform.position);
        _currentTile = TileManager.GetClosetTile(_coordinates);
        _sourroundingTiles = TileManager.GetSouroundingTiles(_currentTile);
    }
}