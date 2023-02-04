using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CliffSpriteLibrary", menuName = "ScriptableObjects/CliffSpriteLibrary", order = 1)]
public class CliffSpriteLibrary : ScriptableObject
{
    [SerializeField] private Sprite[] _sprites;

    [SerializeField] private Sprite[,,,,,,,] _sortedSprites = new Sprite[2, 2, 2, 2, 2, 2, 2, 2];

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
    }

    public Sprite GetSortedSprite(WaterMask waterMask)
    {
        return _sortedSprites
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
    }
}
