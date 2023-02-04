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

        UpdateCliffSprite();
    }

    private void OnDisable()
    {
        _activeCliffControllers.Remove(this);
    }

    [ContextMenu(nameof(UpdateCliffSprite))]
    public void UpdateCliffSprite()
    {
        if (GameManager.GameState != GameState.Ingame)
        {
            Debug.Log("Not Ingame");

            return;
        }

        SoroundingTiles soroundingTiles = TileManager.GetSouroundingTilesElements(GetComponentInParent<Tile>());

        WaterMask waterMask = new WaterMask(soroundingTiles);

        _tileElement.SpriteRenderer.sprite = CliffManager.CliffSpriteLibrary().GetSortedSprite(waterMask);
    }
}
