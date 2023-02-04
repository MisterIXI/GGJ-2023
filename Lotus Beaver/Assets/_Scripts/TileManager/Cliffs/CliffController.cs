using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileElement))]
public class CliffController : MonoBehaviour
{
    public static List<CliffController> _activeCliffControllers = new List<CliffController>();

    private TileElement _tileElement;

    private Tile _parentTile;

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

    private void Update()
    {
        // for (int i = 0; i < _activeCliffControllers.Count; i++)
        // {
        //     UpdateCliffSprite();
        // }
    }

    [ContextMenu(nameof(UpdateCliffSprite))]
    public void UpdateCliffSprite()
    {
        // if (GameManager.GameState != GameState.Ingame)
        // {
        //     Debug.Log("Not Ingame");

        //     return;
        // }

        Tile parentTile = GetComponentInParent<Tile>(true);

        SurroundingTiles soroundingTiles = TileManager.GetSurroundingTilesElements(parentTile);

        WaterMask waterMask = new WaterMask(soroundingTiles);

        _tileElement.SpriteRenderer.sprite = CliffManager.CliffSpriteLibrary()?.GetSortedSprite(waterMask);
    }

    
}
