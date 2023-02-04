using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CliffSpriteLibrary", menuName = "ScriptableObjects/CliffSpriteLibrary", order = 1)]
public class CliffSpriteLibrary : ScriptableObject
{
    [SerializeField] private Sprite[] _sprites;

    private Sprite[,,,,,,,] _sortedSprites;

    [SerializeField] private char _waterChar;
    [SerializeField] private char _bothChar;

    [ContextMenu(nameof(LoadNewSpirts))]
    private void LoadNewSpirts()
    {
        _sortedSprites = new Sprite[2, 2, 2, 2, 2, 2, 2, 2];

        foreach (Sprite sprite in _sprites)
        {
            WaterMask waterMask = new()
            {
                Up = sprite.name[0] == _waterChar ? true : false,
                UpRight = sprite.name[2] == _waterChar ? true : false,
                Right = sprite.name[4] == _waterChar ? true : false,
                DownRight = sprite.name[6] == _waterChar ? true : false,
                Down = sprite.name[8] == _waterChar ? true : false,
                DownLeft = sprite.name[10] == _waterChar ? true : false,
                Left = sprite.name[12] == _waterChar ? true : false,
                UpLeft = sprite.name[14] == _waterChar ? true : false,
            };

            _sortedSprites[
                waterMask.Up? 1 : 0,
                waterMask.UpRight ? 1 : 0,
                waterMask.Right ? 1 : 0,
                waterMask.DownRight ? 1 : 0,
                waterMask.Down ? 1 : 0,
                waterMask.DownLeft ? 1 : 0,
                waterMask.Left ? 1 : 0,
                waterMask.UpLeft ? 1 : 0
            ] = sprite;

            int bothCharCount = sprite.name.ToCharArray().Count(x => x == _bothChar);

            for (int i = 0; i < bothCharCount; i++)
            {
                char[] spriteNameCharArray = sprite.name.ToCharArray();

                int bothIndex = Array.IndexOf(spriteNameCharArray, spriteNameCharArray.Skip(i).FirstOrDefault(x => x == _bothChar));

                spriteNameCharArray[bothIndex] = _waterChar;

                string spriteName = new string(spriteNameCharArray);

                waterMask = new()
                {
                    Up = spriteName[0] == _waterChar ? true : false,
                    UpRight = spriteName[2] == _waterChar ? true : false,
                    Right = spriteName[4] == _waterChar ? true : false,
                    DownRight = spriteName[6] == _waterChar ? true : false,
                    Down = spriteName[8] == _waterChar ? true : false,
                    DownLeft = spriteName[10] == _waterChar ? true : false,
                    Left = spriteName[12] == _waterChar ? true : false,
                    UpLeft = spriteName[14] == _waterChar ? true : false,
                };

                _sortedSprites[
                    waterMask.Up ? 1 : 0,
                    waterMask.UpRight ? 1 : 0,
                    waterMask.Right ? 1 : 0,
                    waterMask.DownRight ? 1 : 0,
                    waterMask.Down ? 1 : 0,
                    waterMask.DownLeft ? 1 : 0,
                    waterMask.Left ? 1 : 0,
                    waterMask.UpLeft ? 1 : 0
                ] = sprite;
            }

            if (bothCharCount == 2)
            {
                string spriteName = sprite.name;

                spriteName.Replace(_bothChar, _waterChar);

                waterMask = new()
                {
                    Up = spriteName[0] == _waterChar ? true : false,
                    UpRight = spriteName[2] == _waterChar ? true : false,
                    Right = spriteName[4] == _waterChar ? true : false,
                    DownRight = spriteName[6] == _waterChar ? true : false,
                    Down = spriteName[8] == _waterChar ? true : false,
                    DownLeft = spriteName[10] == _waterChar ? true : false,
                    Left = spriteName[12] == _waterChar ? true : false,
                    UpLeft = spriteName[14] == _waterChar ? true : false,
                };

                _sortedSprites[
                    waterMask.Up ? 1 : 0,
                    waterMask.UpRight ? 1 : 0,
                    waterMask.Right ? 1 : 0,
                    waterMask.DownRight ? 1 : 0,
                    waterMask.Down ? 1 : 0,
                    waterMask.DownLeft ? 1 : 0,
                    waterMask.Left ? 1 : 0,
                    waterMask.UpLeft ? 1 : 0
                ] = sprite;
            }
        }

        for (int a0 = 0; a0 < _sortedSprites.GetLength(0); a0++)
        {
            for (int a1 = 0; a1 < _sortedSprites.GetLength(0); a1++)
            {
                for (int a2 = 0; a2 < _sortedSprites.GetLength(0); a2++)
                {
                    for (int a3 = 0; a3 < _sortedSprites.GetLength(0); a3++)
                    {
                        for (int a4 = 0; a4 < _sortedSprites.GetLength(0); a4++)
                        {
                            for (int a5 = 0; a5 < _sortedSprites.GetLength(0); a5++)
                            {
                                for (int a6 = 0; a6 < _sortedSprites.GetLength(0); a6++)
                                {
                                    for (int a7 = 0; a7 < _sortedSprites.GetLength(0); a7++)
                                    {
                                        Debug.Log($"{a0}{a1}{a2}{a3}{a4}{a5}{a6}{a7} {_sortedSprites[a0, a1, a2, a3, a4, a5, a6, a7]?.name}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public Sprite GetSortedSprite(WaterMask waterMask)
    {
        if (_sortedSprites == null || _sortedSprites.Length == 0)
        {
            LoadNewSpirts();
        }

        Sprite sprite = _sortedSprites
            [
                waterMask.Up? 1 : 0,
                waterMask.UpRight ? 1 : 0,
                waterMask.Right ? 1 : 0,
                waterMask.DownRight ? 1 : 0,
                waterMask.Down ? 1 : 0,
                waterMask.DownLeft ? 1 : 0,
                waterMask.Left ? 1 : 0,
                waterMask.UpLeft ? 1 : 0
            ];

        Debug.Log($"{sprite?.name} {waterMask}");

        return sprite;
    }
}
