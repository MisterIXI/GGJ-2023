using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileElement))]
public class WaterController : MonoBehaviour
{
    public static List<WaterController> _activeWaterControllers = new List<WaterController>();

    private TileElement _tileElement;

    private void Awake()
    {
        _tileElement = GetComponent<TileElement>();
    }

    private void OnEnable()
    {
        _activeWaterControllers.Add(this);
    }

    private void OnDisable()
    {
        _activeWaterControllers.Remove(this);
    }

    [ContextMenu(nameof(UpdateWater))]
    public void UpdateWater()
    {

    }

    [ContextMenu(nameof(MakeEarth))]
    public void MakeEarth()
    {
        Tile tile = GetComponentInParent<Tile>();

        TileManager.SetTileElementType(tile, TileElementType.Earth);
    }

    [ContextMenu(nameof(MakeCliff))]
    public void MakeCliff()
    {
        Tile tile = GetComponentInParent<Tile>();

        TileManager.SetTileElementType(tile, TileElementType.Cliff);
    }
}