public class TilePool : Pool<Tile>
{
    protected override void SetPool(Tile poolable)
    {
        poolable.SetPool(this);
    }
}