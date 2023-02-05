using UnityEngine;

public class TileElement : Poolable
{
    [SerializeField] private TileElementType _tileElementType;
    public TileElementType TileElementType => _tileElementType;

    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; set; }

    private TileElementPool _pool;

    public void SetPool(TileElementPool pool)
    {
        _pool = pool;
    }

    public override void ReturnToPool()
    {
        _pool.ReturnToPool(this);
    }

    [ContextMenu("ChangeToWater")]
    public void ChangeToWater()
    {
        TileManager.SetTileElementType(GetComponentInParent<Tile>(), TileElementType.Water);
    }

    [ContextMenu("ChangeToEart")]
    public void ChangeToEarth()
    {
        TileManager.SetTileElementType(GetComponentInParent<Tile>(), TileElementType.Earth);
    }

    [ContextMenu("ChangeToCliff")]
    public void ChangeToCliff()
    {
        TileManager.SetTileElementType(GetComponentInParent<Tile>(), TileElementType.Cliff);
    }

}