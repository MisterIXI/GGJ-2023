using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CliffSpriteLibrary", menuName = "ScriptableObjects/CliffSpriteLibrary", order = 1)]
public class CliffSpriteLibrary : ScriptableObject
{
    [SerializeField] private Sprite[] _sprites;

    private Sprite[,,,,,,,] _sortedSprites;

    [SerializeField] private char _waterChar;
    [SerializeField] private char _bothChar;
    [SerializeField] private char _earthChar;

    [ContextMenu(nameof(LoadNewSpirts))]
    private void LoadNewSpirts()
    {
        _sortedSprites = new Sprite[2, 2, 2, 2, 2, 2, 2, 2];

        foreach (Sprite sprite in _sprites)
            RecReplaceChar(sprite, sprite.name.ToCharArray());

        // DebugPrint();
        CountEmpties();
    }

    private void RecReplaceChar(Sprite sprite, char[] name)
    {
        int bothCharCount = name.Count(x => x == _bothChar);
        if (bothCharCount == 0)
        {
            SetSprite(sprite, new string(name));
        }
        else
        {
            // get index of first both char
            int bothIndex = Array.IndexOf(name, name.FirstOrDefault(x => x == _bothChar));
            name[bothIndex] = _waterChar;
            RecReplaceChar(sprite, name.Clone() as char[]);
            name[bothIndex] = _earthChar;
            RecReplaceChar(sprite, name.Clone() as char[]);
        }
    }

    private void SetSprite(Sprite sprite, string name)
    {
        WaterMask waterMask = new()
        {
            UpLeft = name[0] == _waterChar ? true : false,
            Up = name[2] == _waterChar ? true : false,
            UpRight = name[4] == _waterChar ? true : false,
            Right = name[6] == _waterChar ? true : false,
            DownRight = name[8] == _waterChar ? true : false,
            Down = name[10] == _waterChar ? true : false,
            DownLeft = name[12] == _waterChar ? true : false,
            Left = name[14] == _waterChar ? true : false,
        };
        _sortedSprites[
            waterMask.UpLeft ? 1 : 0,
            waterMask.Up ? 1 : 0,
            waterMask.UpRight ? 1 : 0,
            waterMask.Right ? 1 : 0,
            waterMask.DownRight ? 1 : 0,
            waterMask.Down ? 1 : 0,
            waterMask.DownLeft ? 1 : 0,
            waterMask.Left ? 1 : 0
        ] = sprite;
    }
    private void CountEmpties()
    {
        int count = 0;
        List<String> emptyNames = new List<string>();
        // count all empty sprites in _sortedSprites
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
                                        if (_sortedSprites[a0, a1, a2, a3, a4, a5, a6, a7] == null)
                                        {
                                            count++;
                                            emptyNames.Add($"{a0}{a1}{a2}{a3}{a4}{a5}{a6}{a7}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("Missing Entries: " + count);
        Debug.Log("Missing Entries: " + string.Join(", ", emptyNames.ToArray()));
    }
    private void DebugPrint()
    {
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
                waterMask.UpLeft ? 1 : 0,
                waterMask.Up ? 1 : 0,
                waterMask.UpRight ? 1 : 0,
                waterMask.Right ? 1 : 0,
                waterMask.DownRight ? 1 : 0,
                waterMask.Down ? 1 : 0,
                waterMask.DownLeft ? 1 : 0,
                waterMask.Left ? 1 : 0
            ];

        // Debug.Log($"{sprite?.name} {waterMask}");

        return sprite;
    }
}
