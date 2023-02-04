using System;
using UnityEngine;

public struct WaterMask
{
    public bool UpLeft;
    public bool Up;
    public bool UpRight;
    public bool Right;
    public bool DownRight;
    public bool Down;
    public bool DownLeft;
    public bool Left;

    public WaterMask(bool upLeft, bool up, bool upRight, bool right, bool downRight, bool down, bool downLeft, bool left)
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

    public WaterMask(SoroundingTiles sorounding)
    {
        UpLeft = sorounding.UpLeft?.TileElementType == TileElementType.Water;
        Up = sorounding.Up?.TileElementType == TileElementType.Water;
        UpRight = sorounding.UpRight?.TileElementType == TileElementType.Water;
        Right = sorounding.Right?.TileElementType == TileElementType.Water;
        DownRight = sorounding.DownRight?.TileElementType == TileElementType.Water;
        Down = sorounding.Down?.TileElementType == TileElementType.Water;
        DownLeft = sorounding.DownLeft?.TileElementType == TileElementType.Water;
        Left = sorounding.Left?.TileElementType == TileElementType.Water;
    }

    public override string ToString()
    {
        return
                $"UpLeft: {UpLeft}" +
                $"Up: {Up}" +
                $"UpRight: {UpRight}" +
                $"Right: {Right}" +
                $"DownRight: {DownRight}" +
                $"Down: {Down}" +
                $"DownLeft: {DownLeft}" +
                $"Left: {Left}";
    }
}