using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileElement))]
public class CliffController : TileController
{
    public static List<CliffController> _activeCliffControllers = new();
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
        _ = StartCoroutine(DelayedUpdate());
        UpdateCliffSprite();
    }

    private IEnumerator DelayedUpdate()
    {
        yield return new WaitForEndOfFrame();
        UpdateCliffSprite();
    }

    private void OnDisable()
    {
        _ = _activeCliffControllers.Remove(this);
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

        WaterMask waterMask = new(soroundingTiles);

        _tileElement.SpriteRenderer.sprite = CliffManager.CliffSpriteLibrary()?.GetSortedSprite(waterMask);
        // Debug.Log("Cliff Update: " + waterMask);
    }
}