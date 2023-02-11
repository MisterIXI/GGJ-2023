using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileElement))]
public class CliffController : TileController
{
    public static List<CliffController> _activeCliffControllers = new();

    private void OnEnable()
    {
        _activeCliffControllers.Add(this);
        _ = StartCoroutine(DelayedUpdate());
        UpdateCliffSprite();
    }

    private IEnumerator DelayedUpdate()
    {
        yield return new WaitForEndOfFrame();
        UpdateCliffSprite();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _ = _activeCliffControllers.Remove(this);
    }


    [ContextMenu(nameof(UpdateCliffSprite))]
    public void UpdateCliffSprite()
    {
        SurroundingTiles soroundingTiles = TileManager.GetSurroundingTilesElements(ParentTile);

        WaterMask waterMask = new(soroundingTiles);

        _tileElement.SpriteRenderer.sprite = CliffManager.CliffSpriteLibrary()?.GetSortedSprite(waterMask);
    }
}