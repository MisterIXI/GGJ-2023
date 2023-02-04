public class TileElementPool : Pool<TileElement>
{
    protected override void Start()
    {
        TileManager.AddTileElementPool(Peek().TileElementType, this);
    }

    protected override void SetPool(TileElement poolable)
    {
        poolable.SetPool(this);
    }
}