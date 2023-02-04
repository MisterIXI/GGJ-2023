public struct WaterMask
{
    public bool Up;
    public bool UpRight;
    public bool Right;
    public bool DownRight;
    public bool Down;
    public bool DownLeft;
    public bool Left;
    public bool UpLeft;

    public WaterMask(bool up, bool upRight, bool right, bool downRight, bool down, bool downLeft, bool left, bool upLeft)
    {
        Up = up;
        UpRight = upRight;
        Right = right;
        DownRight = downRight;
        Down = down;
        DownLeft = downLeft;
        Left = left;
        UpLeft = upLeft;
    }

    public WaterMask(SoroundingTiles sorounding)
    {
        Up = sorounding.Up?.TileElementType == TileElementType.Water;
        UpRight = sorounding.UpRight?.TileElementType == TileElementType.Water;
        Right = sorounding.Right?.TileElementType == TileElementType.Water;
        DownRight = sorounding.DownRight?.TileElementType == TileElementType.Water;
        Down = sorounding.Down?.TileElementType == TileElementType.Water;
        DownLeft = sorounding.DownLeft?.TileElementType == TileElementType.Water;
        Left = sorounding.Left?.TileElementType == TileElementType.Water;
        UpLeft = sorounding.UpLeft?.TileElementType == TileElementType.Water;
    }
}