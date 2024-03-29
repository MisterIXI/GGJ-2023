﻿using UnityEngine;
using System.Collections.Generic;

public class CliffManager : MonoBehaviour
{
    [SerializeField] private CliffSpriteLibrary _cliffSpriteLibrary;
    [SerializeField] private DamageStageLibrary _damageStageLibrary;

    private static CliffManager _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(transform.root.gameObject);
        TickManager.OnDamageTick += OnDamageTickCliffs;
    }

    public void OnDamageTickCliffs(object sender, TickManager.OnTickEventArgs e)
    {
        var cliffControllers = CliffController._activeCliffControllers.ToArray();
        foreach (CliffController cliffC in cliffControllers)
        {
            // get Tile of cliffController
            if (!cliffC.isActiveAndEnabled)
                continue;
            Tile currentTile = cliffC.ParentTile;
            List<Tile> tilesToCheck = TileManager.GetSurroundingTilesWithDiagonal(currentTile);
            // filter list to only include tiles with TileElementTypes.Earth
            tilesToCheck.RemoveAll(tile => tile.TileElement.TileElementType != TileElementType.Earth);
            foreach (Tile tile in tilesToCheck)
            {
                tile.TileElement.GetComponent<EarthController>().LoseHealth(_damageStageLibrary.BaseDamage);
            }

        }
    }

    public static CliffSpriteLibrary CliffSpriteLibrary()
    {
        if (_instance == null)
        {
            return null;
        }

        return _instance._cliffSpriteLibrary;
    }
}
