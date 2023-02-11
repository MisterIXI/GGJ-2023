using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private void OnDestroy()
    {
        TickManager.OnDamageTick -= OnDamageTickCliffs;
    }

    public void OnDamageTickCliffs(object sender, TickManager.OnTickEventArgs e)
    {
        foreach (CliffController cliffController in CliffController._activeCliffControllers.ToArray())
        {
            if (!cliffController.isActiveAndEnabled)
            {
                continue;
            }

            Tile currentTile = cliffController.ParentTile;

            foreach (EarthController earthController in TileManager.GetSurroundingTilesWithDiagonal(currentTile).Select(x => x.TileElement.TileController as EarthController).ToList())
            {
                earthController?.LoseHealth(_damageStageLibrary.BaseDamage);
            }
        }
    }

    public static CliffSpriteLibrary CliffSpriteLibrary()
    {
        return _instance == null ? null : _instance._cliffSpriteLibrary;
    }
}