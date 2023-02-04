public struct SoroundingTiles
{
    public TileElement UpLeft;
    public TileElement Up;
    public TileElement UpRight;
    public TileElement Right;
    public TileElement DownRight;
    public TileElement Down;
    public TileElement DownLeft;
    public TileElement Left;

    public SoroundingTiles(TileElement upLeft, TileElement up, TileElement upRight, TileElement right, TileElement downRight, TileElement down, TileElement downLeft, TileElement left)
    {
        UpLeft = upLeft;
        Up = up;
        UpRight = upRight;
        Right = right;
        DownRight = downRight;
        Down = down;
        DownLeft = downLeft;
        Left = left;
    }
}