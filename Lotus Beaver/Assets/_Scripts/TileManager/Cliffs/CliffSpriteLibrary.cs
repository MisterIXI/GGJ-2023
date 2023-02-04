using UnityEngine;

[CreateAssetMenu(fileName = "CliffSpriteLibrary", menuName = "ScriptableObjects/CliffSpriteLibrary", order = 1)]
public class CliffSpriteLibrary : ScriptableObject
{
    [SerializeField] private Sprite[] _sprites;

    [SerializeField] private Sprite[,,,,,,,] _sortedSprites = new Sprite[2, 2, 2, 2, 2, 2, 2, 2];

    [SerializeField] private char _waterChar;

    [ContextMenu(nameof(LoadNewSpirts))]
    private void LoadNewSpirts()
    {
        _sortedSprites = new Sprite[2, 2, 2, 2, 2, 2, 2, 2];

        foreach (Sprite sprite in _sprites)
        {
            WaterMask waterMask = new()
            {
                Up = sprite.name[0] == _waterChar ? true : false,
                UpRight = sprite.name[1] == _waterChar ? true : false,
                Right = sprite.name[2] == _waterChar ? true : false,
                DownRight = sprite.name[3] == _waterChar ? true : false,
                Down = sprite.name[4] == _waterChar ? true : false,
                DownLeft = sprite.name[5] == _waterChar ? true : false,
                Left = sprite.name[6] == _waterChar ? true : false,
                UpLeft = sprite.name[7] == _waterChar ? true : false,
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
