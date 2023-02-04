using UnityEngine;

public class TileElement : Poolable
{
    [SerializeField] private TileElementType _tileElementType;
    public TileElementType TileElementType => _tileElementType;
    private Sprite _sprite;

    private TileElementPool _pool;

    public void SetPool(TileElementPool pool)
    {
        _pool = pool;
    }

    public override void ReturnToPool()
    {
        _pool.ReturnToPool(this);
    }
}