using UnityEngine;

[RequireComponent(typeof(TileElement))]
public class CliffController : MonoBehaviour
{
    private TileElement _tileElement;

    private void Awake()
    {
        _tileElement = GetComponent<TileElement>();
    }

    public void UpdateCliffSprite()
    {
        SoroundingTiles soroundingTiles = TileManager.GetSouroundingTilesElements(GetComponent<Tile>());

        WaterMask waterMask = new WaterMask(soroundingTiles);

        _tileElement.SpriteRenderer.sprite = CliffManager.CliffSpriteLibrary().GetSortedSprite(waterMask);
    }
}
