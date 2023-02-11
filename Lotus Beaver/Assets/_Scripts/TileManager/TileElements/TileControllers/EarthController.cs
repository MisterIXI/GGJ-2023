using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TileElement))]
public class EarthController : TileController
{
    [SerializeField] private float _health;
    [SerializeField] private DamageStageLibrary _damageStageLibrary;
    [SerializeField] private DamageStage _currentDamageStage;

    private Tile _parentTile;
    private TileElement _tileElement;

    private void Awake()
    {
        _tileElement = GetComponent<TileElement>();
    }

    private void OnEnable()
    {
        _parentTile = GetComponentInParent<Tile>(true);

        _health = _damageStageLibrary.EarhMaxHealth;

        _currentDamageStage = _damageStageLibrary.DamageStages[0];
        if (Time.time - GameManager.GameStartTime > 0.5f)
        {
            _ = (RefManager.gameManager?.StartCoroutine(DelayRoutine()));
        }

        UpdateDamageSprite();
    }

    private IEnumerator DelayRoutine()
    {
        yield return null;

        TileCreationPool.ParticlePool?.GetPoolable()?.Play(transform.position);
    }

    private void UpdateDamageStage()
    {
        DamageStage damageStage = _damageStageLibrary.GetCurrentDamageStage(_health);

        if (_currentDamageStage == damageStage)
        {
            return;
        }

        _currentDamageStage = damageStage;

        UpdateDamageSprite();
    }

    private void UpdateDamageSprite()
    {
        _tileElement.SpriteRenderer.sprite = _currentDamageStage.Sprite;
    }

    [ContextMenu(nameof(TestTakeDamage))]
    private void TestTakeDamage()
    {
        LoseHealth(25f);
    }

    [ContextMenu(nameof(GetHealth))]
    private void GetHealth()
    {
        GetHealth(25f);
    }

    public void LoseHealth(float healthPoints)
    {
        _health -= healthPoints;

        if (_health <= 0f)
        {
            TakeFatalDamage();
            return;
        }

        UpdateDamageStage();
    }

    public void GetHealth(float healthPoints)
    {
        _health = Mathf.Min(_damageStageLibrary.EarhMaxHealth, _health + healthPoints);

        UpdateDamageStage();
    }

    private void TakeFatalDamage()
    {
        Tile parentTile = GetComponentInParent<Tile>(true);

        if (TileManager.IsSurroundedByWaterOrCliff(parentTile))
        {
            TileManager.SetTileElementType(parentTile, TileElementType.Water);
        }
        else
        {
            TileManager.SetTileElementType(parentTile, TileElementType.Cliff);
        }
        SoundManager.PlayWaterSplash();
        if (Time.time - GameManager.GameStartTime > 0.5f)
        {
            TileDestroyPool.ParticlePool?.GetPoolable()?.Play(transform.position);
        }
    }
}