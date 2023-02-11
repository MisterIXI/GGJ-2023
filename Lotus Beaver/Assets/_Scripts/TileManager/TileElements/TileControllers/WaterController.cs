using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileElement))]
public class WaterController : TileController
{
    public static List<WaterController> _activeWaterControllers = new();

    private void OnEnable()
    {
        _activeWaterControllers.Add(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _ = _activeWaterControllers.Remove(this);
    }

#if UNITY_EDITOR
    [ContextMenu(nameof(MakeEarth))]
    public void MakeEarth()
    {
        TileManager.SetTileElementType(ParentTile, TileElementType.Earth);
    }

    [ContextMenu(nameof(MakeCliff))]
    public void MakeCliff()
    {
        TileManager.SetTileElementType(ParentTile, TileElementType.Cliff);
    }
#endif
}