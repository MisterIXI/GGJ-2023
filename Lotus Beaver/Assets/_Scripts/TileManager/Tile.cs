using UnityEngine;

public class Tile : Poolable
{
    public TileElement TileElement { get; set; }

    private TilePool _pool;

    public InteractableBase InteractableBase { get; set; }

    public Vector2Int Coordinates { get; set; }

    public void SetPool(TilePool pool)
    {
        _pool = pool;
    }

    public override void ReturnToPool()
    {
        _pool.ReturnToPool(this);
    }

    public override void SetActive(bool active)
    {
        if (!active && TileElement != null)
        {
            TileElement.Transform.SetParent(null);
            TileElement.SetActive(false);
            TileElement = null;
        }

        _gameObject.SetActive(active);
    }
}