using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileElement))]
public class CliffController : MonoBehaviour
{
    public static List<CliffController> _activeCliffControllers = new List<CliffController>();

    private TileElement _tileElement;

    private void Awake()
    {
        _tileElement = GetComponent<TileElement>();
    }

    private void OnEnable()
    {
        _activeCliffControllers.Add(this);
    }

    private void OnDisable()
    {
        _activeCliffControllers.Remove(this);
    }

    public void UpdateCliffSprite()
    {
        SoroundingTiles soroundingTiles = TileManager.GetSouroundingTilesElements(GetComponent<Tile>());

        WaterMask waterMask = new WaterMask(soroundingTiles);

        _tileElement.SpriteRenderer.sprite = CliffManager.CliffSpriteLibrary().GetSortedSprite(waterMask);
    }
}
