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

    public WaterMask(SurroundingTiles surrounding)
    {
        UpLeft = IsWaterOrCliff(surrounding.UpLeft?.TileElementType);
        Up = IsWaterOrCliff(surrounding.Up?.TileElementType);
        UpRight = IsWaterOrCliff(surrounding.UpRight?.TileElementType);
        Right = IsWaterOrCliff(surrounding.Right?.TileElementType);
        DownRight = IsWaterOrCliff(surrounding.DownRight?.TileElementType);
        Down = IsWaterOrCliff(surrounding.Down?.TileElementType);
        DownLeft = IsWaterOrCliff(surrounding.DownLeft?.TileElementType);
        Left = IsWaterOrCliff(surrounding.Left?.TileElementType);
    }

    private static bool IsWaterOrCliff(TileElementType? tileElementType)
    {
        if (tileElementType == null)
            return true;
        return tileElementType == TileElementType.Water || tileElementType == TileElementType.Cliff;
    }


    public override string ToString()
    {
        // return
        //         $"UpLeft: {UpLeft}  " +
        //         $"Up: {Up}  " +
        //         $"UpRight: {UpRight}  " +
        //         $"Right: {Right}  " +
        //         $"DownRight: {DownRight}  " +
        //         $"Down: {Down}  " +
        //         $"DownLeft: {DownLeft}  " +
        //         $"Left: {Left}  ";
        return $"{(UpLeft ? 1 : 0)}{(Up ? 1 : 0)}{(UpRight ? 1 : 0)}{(Right ? 1 : 0)}{(DownRight ? 1 : 0)}{(Down ? 1 : 0)}{(DownLeft ? 1 : 0)}{(Left ? 1 : 0)}";
    }
}