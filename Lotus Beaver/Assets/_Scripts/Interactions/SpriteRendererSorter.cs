using UnityEngine;

public class SpriteRendererSorter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public void SetSortOrderFromCoordinate(Tile tile)
    {
        TileManager.GetSortOrderFromCoordinate(tile.Coordinates);
    }
}
