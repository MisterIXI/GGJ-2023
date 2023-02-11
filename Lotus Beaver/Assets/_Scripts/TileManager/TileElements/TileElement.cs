using UnityEngine;

public class TileElement : Poolable
{
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
    [SerializeField] private TileElementType _tileElementType;
    [SerializeField] private TileController _tileController;
    public TileElementType TileElementType => _tileElementType;
    public TileController TileController => _tileController;

    private TileElementPool _pool;

    public void SetPool(TileElementPool pool)
    {
        _pool = pool;
    }

    public override void ReturnToPool()
    {
        _pool.ReturnToPool(this);
    }

#if UNITY_EDITOR
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
#endif
}