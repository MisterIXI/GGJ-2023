using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileElement))]
public class CliffController : MonoBehaviour
{
    public static List<CliffController> _activeCliffControllers = new List<CliffController>();
    public Tile ParentTile => _parentTile ??= GetComponentInParent<Tile>(true);
    private Tile _parentTile;
    private TileElement _tileElement;
    private void Awake()
    {
        _tileElement = GetComponent<TileElement>();
    }

    private void OnEnable()
    {
        _activeCliffControllers.Add(this);
        _parentTile = GetComponentInParent<Tile>(true);
    }

    private void OnDisable()
    {
        _activeCliffControllers.Remove(this);
        _parentTile = null;
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
        Debug.Log("Cliff Update: " + waterMask);
    }

    
}
